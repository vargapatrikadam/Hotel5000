using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Core.Helpers;
using Core.Enums.Lodging;
using Core.Interfaces.PasswordHasher;

namespace Core.Services.Lodging
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAsyncRepository<Contact> _contactRepository;
        private readonly IAsyncRepository<ApprovingData> _approvingDataRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAsyncRepository<Role> _roleRepository;
        public UserManagementService(IAsyncRepository<Contact> contactRepository, IAsyncRepository<ApprovingData> approvingDataRepository, IAsyncRepository<User> userRepository, IPasswordHasher passwordHasher, IAsyncRepository<Role> roleRepository)
        {
            _contactRepository = contactRepository;
            _approvingDataRepository = approvingDataRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task<Result<bool>> AddApprovingData(ApprovingData approvingData, int userId)
        {
            if (!(await _userRepository.AnyAsync(p => p.Id == userId)))
                return new NotFoundResult<bool>("User not found");

            if (await _approvingDataRepository.AnyAsync(p => p.UserId == userId))
                return new InvalidResult<bool>("User already has approving data");

            if (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == approvingData.IdentityNumber
                    || p.RegistrationNumber == approvingData.RegistrationNumber
                    || p.TaxNumber == approvingData.TaxNumber))
                return new InvalidResult<bool>("Approving data not unique");

            approvingData.UserId = userId;
            await _approvingDataRepository.AddAsync(approvingData);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddContact(Contact contact, int userId)
        {
            if (!(await _userRepository.AnyAsync(p => p.Id == userId)))
                return new NotFoundResult<bool>("User not found");

            if (await _contactRepository.AnyAsync(p => p.MobileNumber == contact.MobileNumber))
                return new InvalidResult<bool>("Mobile number already exists");

            contact.UserId = userId;
            await _contactRepository.AddAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddUser(User user, string role)
        {
            if (await _userRepository.AnyAsync(p => p.Email == user.Email))
                return new InvalidResult<bool>("Email not unique");

            if (await _userRepository.AnyAsync(p => p.Username == user.Username))
                return new InvalidResult<bool>("Username not unique");

            user.Email.ValidateEmail(out var errorMessage);
            user.Password.ValidatePassword(out errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage);

            user.Password = _passwordHasher.Hash(user.Password);

            Roles roleAsEnum;
            if (!Enum.TryParse(role, out roleAsEnum))
            {
                return new InvalidResult<bool>("Role not found");
            }

            var roleEntity = (await _roleRepository.GetAsync(new Specification<Role>().ApplyFilter(p => p.Name == roleAsEnum))).FirstOrDefault();

            user.Role = null;

            user.RoleId = roleEntity.Id;

            await _userRepository.AddAsync(user);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<IReadOnlyList<ApprovingData>>> GetAllApprovingData()
        {
            return new SuccessfulResult<IReadOnlyList<ApprovingData>>(await _approvingDataRepository.GetAllAsync());
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetAllContacts()
        {
            return new SuccessfulResult<IReadOnlyList<Contact>>(await _contactRepository.GetAllAsync());
        }

        public async Task<Result<IReadOnlyList<User>>> GetAllUsers()
        {
            return new SuccessfulResult<IReadOnlyList<User>>(await _userRepository.GetAllAsync());
        }

        public async Task<Result<ApprovingData>> GetApprovingData(int userId)
        {
            ApprovingData approvingData = (await _approvingDataRepository.GetAsync(
                    new Specification<ApprovingData>().ApplyFilter(p => p.UserId == userId)))
                    .FirstOrDefault();

            if (approvingData == null)
                return new NotFoundResult<ApprovingData>("Approving data not found");

            return new SuccessfulResult<ApprovingData>(approvingData);
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetContacts(int userId)
        {
            IReadOnlyList<Contact> contacts = await _contactRepository.GetAsync(
                    new Specification<Contact>().ApplyFilter(p => p.UserId == userId));

            if (contacts.Count == 0)
                return new NotFoundResult<IReadOnlyList<Contact>>("Contacts not found for user");

            return new SuccessfulResult<IReadOnlyList<Contact>>(contacts);
        }

        public async Task<Result<User>> GetUser(int userId)
        {
            User user = (await _userRepository.GetAsync(
                new Specification<User>().ApplyFilter(p => p.Id == userId))).FirstOrDefault();

            if (user == null)
                return new NotFoundResult<User>("User not found");

            return new SuccessfulResult<User>(user);
        }

        public async Task<Result<bool>> RemoveApprovingData(int userId)
        {
            ApprovingData approvingData = (await _approvingDataRepository.GetAsync(
                    new Specification<ApprovingData>().ApplyFilter(p => p.UserId == userId))).FirstOrDefault();

            if (approvingData == null)
                return new NotFoundResult<bool>("Approving data not found for user");

            await _approvingDataRepository.DeleteAsync(approvingData);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveContact(int contactId)
        {
            Contact contact = (await _contactRepository.GetAsync(
                    new Specification<Contact>().ApplyFilter(p => p.Id == contactId))).FirstOrDefault();

            if (contact == null)
                return new NotFoundResult<bool>("Contact not found");

            await _contactRepository.DeleteAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveUser(int userId)
        {
            User user = (await _userRepository.GetAsync(
                    new Specification<User>().ApplyFilter(p => p.Id == userId))).FirstOrDefault();

            if (user == null)
                return new NotFoundResult<bool>("User not found");

            await _userRepository.DeleteAsync(user);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateApprovingData(ApprovingData approvingData, int userId, int approvingDataId)
        {
            if (!(await _userRepository.AnyAsync(p => p.Id == userId)))
                return new NotFoundResult<bool>("User not found");

            ApprovingData oldApprovingData = (await _approvingDataRepository.GetAsync(
                    new Specification<ApprovingData>().ApplyFilter(p => p.Id == approvingDataId))).FirstOrDefault();

            if (oldApprovingData == null)
                return new NotFoundResult<bool>("Approving data not found");

            await _approvingDataRepository.UpdateAsync(approvingData);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateContact(Contact contact, int userId, int contactId)
        {
            if (!(await _userRepository.AnyAsync(p => p.Id == userId)))
                return new NotFoundResult<bool>("User not found");

            Contact oldContact = (await _contactRepository.GetAsync(
                    new Specification<Contact>().ApplyFilter(p => p.Id == contactId))).FirstOrDefault();

            if (oldContact == null)
                return new NotFoundResult<bool>("Contact not found");

            await _contactRepository.UpdateAsync(contact);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateUser(User user, int userId)
        {
            User oldUser = (await _userRepository.GetAsync(
                new Specification<User>().ApplyFilter(p => p.Id == userId))).FirstOrDefault();

            if (oldUser == null)
                return new NotFoundResult<bool>("User not found");

            await _userRepository.UpdateAsync(user);

            return new SuccessfulResult<bool>(true);
        }
    }
}
