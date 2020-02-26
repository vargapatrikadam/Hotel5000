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
using Core.Enums;

namespace Core.Services.Lodging
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAsyncRepository<Contact> _contactRepository;
        private readonly IAsyncRepository<ApprovingData> _approvingDataRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAsyncRepository<Role> _roleRepository;
        private readonly IAuthenticationService _authenticationService;
        public UserManagementService(IAsyncRepository<Contact> contactRepository, 
            IAsyncRepository<ApprovingData> approvingDataRepository, 
            IAsyncRepository<User> userRepository, 
            IPasswordHasher passwordHasher, 
            IAsyncRepository<Role> roleRepository,
            IAuthenticationService authenticationService)
        {
            _contactRepository = contactRepository;
            _approvingDataRepository = approvingDataRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _authenticationService = authenticationService;
        }

        public async Task<Result<bool>> AddApprovingData(ApprovingData newApprovingData, 
            int resourceAccessorId)
        {
            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(newApprovingData.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (await _approvingDataRepository.AnyAsync(p => p.UserId == newApprovingData.UserId))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_ALREADY_EXISTS);

            if (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber
                    || p.RegistrationNumber == newApprovingData.RegistrationNumber
                    || p.TaxNumber == newApprovingData.TaxNumber))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            await _approvingDataRepository.AddAsync(newApprovingData);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddContact(Contact contact, 
            int resourceAccessorId)
        {
            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(contact.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (await _contactRepository.AnyAsync(p => p.MobileNumber == contact.MobileNumber))
                return new ConflictResult<bool>(Errors.CONTACT_NOT_UNIQUE);

            await _contactRepository.AddAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddUser(User user, 
            string role)
        {
            if (await _userRepository.AnyAsync(p => p.Email == user.Email))
                return new ConflictResult<bool>(Errors.EMAIL_NOT_UNIQUE);

            if (await _userRepository.AnyAsync(p => p.Username == user.Username))
                return new ConflictResult<bool>(Errors.USERNAME_NOT_UNIQUE);

            user.Email.ValidateEmail(out var errorMessage);
            user.Password.ValidatePassword(out errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage.Value);

            user.Password = _passwordHasher.Hash(user.Password);

            Roles roleAsEnum;
            if (!Enum.TryParse(role, out roleAsEnum))
            {
                return new InvalidResult<bool>(Errors.ROLE_NOT_EXISTS);
            }

            var roleEntity = (await _roleRepository.GetAsync(new Specification<Role>().ApplyFilter(p => p.Name == roleAsEnum))).FirstOrDefault();

            user.Role = null;

            user.RoleId = roleEntity.Id;

            await _userRepository.AddAsync(user);

            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<IReadOnlyList<User>>> GetUsers(int? id = null, 
            string username = null, 
            string email = null,
            int? skip = null,
            int? take = null)
        {
            ISpecification<User> specification = new Specification<User>();
            specification.ApplyFilter(p => 
                (!id.HasValue || p.Id == id) && 
                (username == null || p.Username.Contains(username)) && 
                (email == null || p.Email.Contains(email)))
                .AddInclude(p => p.Role);

            if (skip.HasValue && take.HasValue)
                specification.ApplyPaging(skip.Value, take.Value);

            return new SuccessfulResult<IReadOnlyList<User>>(
                (await _userRepository.GetAsync(specification)).WithoutPasswords().ToList());
        }

        public async Task<Result<IReadOnlyList<ApprovingData>>> GetApprovingData(int? approvingDataOwnerId = null,
            int? approvingDataId = null,
            string username = null,
            string taxNumber = null,
            string identityNumber = null,
            string registrationNumber = null)
        {
            ISpecification<ApprovingData> specification = new Specification<ApprovingData>();
            specification.ApplyFilter(p =>
                (!approvingDataOwnerId.HasValue || p.UserId == approvingDataOwnerId) &&
                (!approvingDataId.HasValue || p.Id == approvingDataId) &&
                (username == null || p.User.Username.Contains(username)) &&
                (taxNumber == null || p.TaxNumber.Contains(taxNumber)) && 
                (identityNumber == null || p.IdentityNumber.Contains(identityNumber)) &&
                (registrationNumber == null || p.RegistrationNumber.Contains(registrationNumber)))
                .AddInclude(i => i.User);

            IReadOnlyList<ApprovingData> approvingDatas = await _approvingDataRepository.GetAsync(specification);

            return new SuccessfulResult<IReadOnlyList<ApprovingData>>(approvingDatas);
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetContacts(int? userId = null, 
            string phoneNumber = null, 
            string username = null)
        {
            ISpecification<Contact> specification = new Specification<Contact>();
            specification.ApplyFilter(p =>
                (!userId.HasValue || p.UserId == userId) &&
                (phoneNumber == null || p.MobileNumber.Contains(phoneNumber)) &&
                (username == null || p.User.Username.Contains(phoneNumber)))
                .AddInclude(i => i.User);

            return new SuccessfulResult<IReadOnlyList<Contact>>(await _contactRepository.GetAsync(specification));
        }
        public async Task<Result<bool>> RemoveApprovingData(int approvingDataOwnerId, 
            int resourceAccessorId)
        {
            ApprovingData approvingData = (await _approvingDataRepository.GetAsync(
                    new Specification<ApprovingData>().ApplyFilter(p => p.UserId == approvingDataOwnerId))).FirstOrDefault();

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(approvingData.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (approvingData == null)
                return new NotFoundResult<bool>(Errors.APPROVING_DATA_NOT_FOUND);

            await _approvingDataRepository.DeleteAsync(approvingData);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> RemoveContact(int contactOwnerId, 
            int contactId, 
            int resourceAccessorId)
        {
            //Contact contact = (await _contactRepository.GetAsync(
            //        new Specification<Contact>().ApplyFilter(p => p.Id == contactId))).FirstOrDefault();
            User user = (await _userRepository.GetAsync(
                    new Specification<User>().ApplyFilter(p => p.Id == contactOwnerId).AddInclude(p => p.Contacts))).FirstOrDefault();

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(user.Id, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            Contact contact = user.Contacts.Where(p => p.Id == contactId).FirstOrDefault();

            if (contact == null)
                return new NotFoundResult<bool>(Errors.CONTACT_NOT_FOUND);

            await _contactRepository.DeleteAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveUser(int userId, 
            int resourceAccessorId)
        {
            User user = (await _userRepository.GetAsync(
                    new Specification<User>().ApplyFilter(p => p.Id == userId))).FirstOrDefault();

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(user.Id, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (user == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            await _userRepository.DeleteAsync(user);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateApprovingData(ApprovingData newApprovingData, 
            int approvingDataId, 
            int resourceAccessorId)
        {
            ApprovingData oldApprovingData = (await _approvingDataRepository.GetAsync(
                    new Specification<ApprovingData>().ApplyFilter(p => p.Id == approvingDataId))).FirstOrDefault();

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(oldApprovingData.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (oldApprovingData == null)
                return new NotFoundResult<bool>(Errors.APPROVING_DATA_NOT_FOUND);

            if (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber
                    || p.RegistrationNumber == newApprovingData.RegistrationNumber
                    || p.TaxNumber == newApprovingData.TaxNumber))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            if ((oldApprovingData.IdentityNumber != newApprovingData.IdentityNumber && 
                    (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber))) ||
                (oldApprovingData.RegistrationNumber != newApprovingData.RegistrationNumber && 
                    (await _approvingDataRepository.AnyAsync(p => p.RegistrationNumber == newApprovingData.RegistrationNumber))) ||
                (oldApprovingData.TaxNumber != newApprovingData.TaxNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.TaxNumber == newApprovingData.TaxNumber))))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            oldApprovingData.IdentityNumber = newApprovingData.IdentityNumber;
            oldApprovingData.RegistrationNumber = newApprovingData.RegistrationNumber;
            oldApprovingData.TaxNumber = newApprovingData.TaxNumber;

            await _approvingDataRepository.UpdateAsync(oldApprovingData);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateContact(Contact newContact, 
            int oldContactId, 
            int resourceAccessorId)
        {
            Contact oldContact = (await _contactRepository.GetAsync(
                    new Specification<Contact>().ApplyFilter(p => p.Id == oldContactId))).FirstOrDefault();

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(oldContact.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (oldContact == null)
                return new NotFoundResult<bool>(Errors.CONTACT_NOT_FOUND);

            if ((oldContact.MobileNumber != newContact.MobileNumber) && 
                (await _contactRepository.AnyAsync(p => p.MobileNumber == newContact.MobileNumber)))
                return new ConflictResult<bool>(Errors.CONTACT_NOT_UNIQUE);

            oldContact.MobileNumber = newContact.MobileNumber;

            await _contactRepository.UpdateAsync(oldContact);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateUser(User newUser, 
            int oldUserId, 
            int resourceAccessorId)
        {
            User oldUser = (await _userRepository.GetAsync(
                new Specification<User>().ApplyFilter(p => p.Id == oldUserId))).FirstOrDefault();

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(oldUser.Id, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (oldUser == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            if ((oldUser.Email != newUser.Email) && await _userRepository.AnyAsync(p => p.Email == newUser.Email))
                return new ConflictResult<bool>(Errors.EMAIL_NOT_UNIQUE);

            if ((oldUser.Username != newUser.Username) && await _userRepository.AnyAsync(p => p.Username == newUser.Username))
                return new ConflictResult<bool>(Errors.USERNAME_NOT_UNIQUE);

            newUser.Email.ValidateEmail(out var errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage.Value);

            oldUser.Email = newUser.Email;
            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.Username = newUser.Username;

            await _userRepository.UpdateAsync(oldUser);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> ChangePassword(int userId, 
            string oldPassword, 
            string newPassword)
        {
            User user = (await _userRepository.GetAsync(
                new Specification<User>().ApplyFilter(p => p.Id == userId))).FirstOrDefault();

            if (user == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            if (!_passwordHasher.Check(user.Password, oldPassword))
                return new InvalidResult<bool>(Errors.PASSWORD_INCORRECT);
            else
            {
                user.Password = _passwordHasher.Hash(newPassword);
                await _userRepository.UpdateAsync(user);
                return new SuccessfulResult<bool>(true);
            }
        }
    }
}
