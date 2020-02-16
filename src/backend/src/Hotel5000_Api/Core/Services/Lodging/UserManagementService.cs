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

namespace Core.Services.Lodging
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAsyncRepository<Contact> _contactRepository;
        private readonly IAsyncRepository<ApprovingData> _approvingDataRepository;
        private readonly IAsyncRepository<User> _userRepository;
        public UserManagementService(IAsyncRepository<Contact> contactRepository, IAsyncRepository<ApprovingData> approvingDataRepository, IAsyncRepository<User> userRepository)
        {
            _contactRepository = contactRepository;
            _approvingDataRepository = approvingDataRepository;
            _userRepository = userRepository;
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

        public async Task<Result<IReadOnlyList<ApprovingData>>> GetAllApprovingData()
        {
            return new SuccessfulResult<IReadOnlyList<ApprovingData>>(await _approvingDataRepository.GetAllAsync());
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetAllContacts()
        {
            return new SuccessfulResult<IReadOnlyList<Contact>>(await _contactRepository.GetAllAsync());
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
    }
}
