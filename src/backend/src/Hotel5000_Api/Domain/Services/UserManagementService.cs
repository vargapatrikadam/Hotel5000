using Core.Entities.Domain;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain;
using Core.Interfaces.Domain.UserManagementService;
using Core.Interfaces.PasswordHasher;
using Core.Results;
using Domain.Services.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IApprovingDataService _approvingDataService;
        private readonly IContactService _contactService;
        private readonly IUserService _userService;
        public UserManagementService(IAsyncRepository<Contact> contactRepository,
            IAsyncRepository<ApprovingData> approvingDataRepository,
            IAsyncRepository<User> userRepository,
            IPasswordHasher passwordHasher,
            IAsyncRepository<Role> roleRepository,
            IAuthorization authorizationService)
        {

            _userService = new UserService(passwordHasher, roleRepository, userRepository, authorizationService);
            _approvingDataService = new ApprovingDataService(approvingDataRepository, authorizationService);
            _contactService = new ContactService(contactRepository, authorizationService, _userService);
        }

        public async Task<Result<bool>> AddApprovingData(ApprovingData newApprovingData)
        {
            return await _approvingDataService.AddApprovingData(newApprovingData);
        }

        public async Task<Result<bool>> AddContact(Contact contact)
        {
            return await _contactService.AddContact(contact);
        }

        public async Task<Result<bool>> AddUser(User user, string role)
        {
            return await _userService.AddUser(user, role);
        }
        public async Task<Result<IReadOnlyList<User>>> GetUsers(int? id = null,
            string username = null,
            string email = null,
            int? skip = null,
            int? take = null)
        {
            return await _userService.GetUsers(id, username, email, skip, take);
        }

        public async Task<Result<IReadOnlyList<ApprovingData>>> GetApprovingData(int? approvingDataOwnerId = null,
            int? approvingDataId = null,
            string username = null,
            string taxNumber = null,
            string identityNumber = null,
            string registrationNumber = null)
        {
            return await _approvingDataService.GetApprovingData(approvingDataOwnerId, approvingDataId, username, taxNumber, identityNumber, registrationNumber);
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetContacts(int? id = null,
            int? userId = null,
            string phoneNumber = null,
            string username = null)
        {
            return await _contactService.GetContacts(id, userId, phoneNumber, username);
        }
        public async Task<Result<bool>> RemoveApprovingData(int approvingDataOwnerId)
        {
            return await _approvingDataService.RemoveApprovingData(approvingDataOwnerId);
        }
        public async Task<Result<bool>> RemoveContact(int contactOwnerId, int contactId)
        {
            return await _contactService.RemoveContact(contactOwnerId, contactId);
        }

        public async Task<Result<bool>> RemoveUser(int userId)
        {
            return await _userService.RemoveUser(userId);
        }

        public async Task<Result<bool>> UpdateApprovingData(ApprovingData newApprovingData,
            int approvingDataId)
        {
            return await _approvingDataService.UpdateApprovingData(newApprovingData, approvingDataId);
        }

        public async Task<Result<bool>> UpdateContact(Contact newContact,
            int contactId)
        {
            return await _contactService.UpdateContact(newContact, contactId);
        }

        public async Task<Result<bool>> UpdateUser(User newUser,
            int userId)
        {
            return await _userService.UpdateUser(newUser, userId);
        }

        public async Task<Result<bool>> ChangePassword(int userId,
            string oldPassword,
            string newPassword)
        {
            return await _userService.ChangePassword(userId, oldPassword, newPassword);
        }
    }
}
