using Core.Entities.Authentication;
using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Authentication;
using Core.Helpers;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.LodgingDomain;
using Core.Interfaces.PasswordHasher;
using Core.Specifications.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.LodgingDomain
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAsyncRepository<Contact> _contactRepository;
        private readonly IAsyncRepository<ApprovingData> _approvingDataRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAsyncRepository<Role> _roleRepository;
        private readonly IAuthorization _authenticationService;
        public UserManagementService(IAsyncRepository<Contact> contactRepository,
            IAsyncRepository<ApprovingData> approvingDataRepository,
            IAsyncRepository<User> userRepository,
            IPasswordHasher passwordHasher,
            IAsyncRepository<Role> roleRepository,
            IAuthorization authenticationService)
        {
            _contactRepository = contactRepository;
            _approvingDataRepository = approvingDataRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _authenticationService = authenticationService;
        }

        public async Task<Result<bool>> AddApprovingData(ApprovingData newApprovingData)
        {
            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ApprovingData)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(newApprovingData.UserId));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (await _approvingDataRepository.AnyAsync(p => p.UserId == newApprovingData.UserId))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_ALREADY_EXISTS);

            if (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber
                    || p.RegistrationNumber == newApprovingData.RegistrationNumber
                    || p.TaxNumber == newApprovingData.TaxNumber))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            await _approvingDataRepository.AddAsync(newApprovingData);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddContact(Contact contact)
        {
            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Contact)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(contact.UserId));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (await _contactRepository.AnyAsync(p => p.MobileNumber == contact.MobileNumber))
                return new ConflictResult<bool>(Errors.CONTACT_NOT_UNIQUE);

            await _contactRepository.AddAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddUser(User user, string role)
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

            RoleType roleAsEnum;
            if (!Enum.TryParse(role, out roleAsEnum))
            {
                return new InvalidResult<bool>(Errors.ROLE_NOT_EXISTS);
            }

            var specification = new GetRole(roleAsEnum);

            var roleEntity = (await _roleRepository.GetAsync(specification)).FirstOrDefault();

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
            var specification = new GetUser(id, username, email, skip, take);

            return new SuccessfulResult<IReadOnlyList<User>>(
                (await _userRepository.GetAsync(specification)));
        }

        public async Task<Result<IReadOnlyList<ApprovingData>>> GetApprovingData(int? approvingDataOwnerId = null,
            int? approvingDataId = null,
            string username = null,
            string taxNumber = null,
            string identityNumber = null,
            string registrationNumber = null)
        {
            var specification = new GetApprovingData(approvingDataOwnerId, approvingDataId, username, taxNumber, identityNumber, registrationNumber);

            IReadOnlyList<ApprovingData> approvingDatas = await _approvingDataRepository.GetAsync(specification);

            return new SuccessfulResult<IReadOnlyList<ApprovingData>>(approvingDatas);
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetContacts(int? id = null,
            int? userId = null,
            string phoneNumber = null,
            string username = null)
        {
            var specification = new GetContacts(id, userId, phoneNumber, username);

            return new SuccessfulResult<IReadOnlyList<Contact>>(await _contactRepository.GetAsync(specification));
        }
        public async Task<Result<bool>> RemoveApprovingData(int approvingDataOwnerId)
        {
            ApprovingData approvingData = (await GetApprovingData(approvingDataOwnerId: approvingDataOwnerId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ApprovingData)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(approvingData.UserId));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (approvingData == null)
                return new NotFoundResult<bool>(Errors.APPROVING_DATA_NOT_FOUND);

            await _approvingDataRepository.DeleteAsync(approvingData);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> RemoveContact(int contactOwnerId, int contactId)
        {
            User user = (await GetUsers(id: contactOwnerId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Contact)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(user.Id));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            Contact contact = user.Contacts.Where(p => p.Id == contactId).FirstOrDefault();

            if (contact == null)
                return new NotFoundResult<bool>(Errors.CONTACT_NOT_FOUND);

            await _contactRepository.DeleteAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveUser(int userId)
        {
            User user = (await GetUsers(id: userId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(User)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(user.Id));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (user == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            await _userRepository.DeleteAsync(user);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateApprovingData(ApprovingData newApprovingData,
            int approvingDataId)
        {
            ApprovingData updateThisApprovingData = (await GetApprovingData(approvingDataId: approvingDataId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ApprovingData)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(updateThisApprovingData.UserId));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (updateThisApprovingData == null)
                return new NotFoundResult<bool>(Errors.APPROVING_DATA_NOT_FOUND);

            if ((updateThisApprovingData.IdentityNumber != newApprovingData.IdentityNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber))) ||
                (updateThisApprovingData.RegistrationNumber != newApprovingData.RegistrationNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.RegistrationNumber == newApprovingData.RegistrationNumber))) ||
                (updateThisApprovingData.TaxNumber != newApprovingData.TaxNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.TaxNumber == newApprovingData.TaxNumber))))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            updateThisApprovingData.IdentityNumber = newApprovingData.IdentityNumber;
            updateThisApprovingData.RegistrationNumber = newApprovingData.RegistrationNumber;
            updateThisApprovingData.TaxNumber = newApprovingData.TaxNumber;

            await _approvingDataRepository.UpdateAsync(updateThisApprovingData);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateContact(Contact newContact,
            int contactId)
        {
            Contact updateThisContact = (await GetContacts(id: contactId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Contact)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(updateThisContact.UserId));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (updateThisContact == null)
                return new NotFoundResult<bool>(Errors.CONTACT_NOT_FOUND);

            if ((updateThisContact.MobileNumber != newContact.MobileNumber) &&
                (await _contactRepository.AnyAsync(p => p.MobileNumber == newContact.MobileNumber)))
                return new ConflictResult<bool>(Errors.CONTACT_NOT_UNIQUE);

            updateThisContact.MobileNumber = newContact.MobileNumber;

            await _contactRepository.UpdateAsync(updateThisContact);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateUser(User newUser,
            int userId)
        {
            User updateThisUser = (await GetUsers(id: userId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(User)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(updateThisUser.Id));
            Result<bool> authorizationResult = await _authenticationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (updateThisUser == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            if ((updateThisUser.Email != newUser.Email) && await _userRepository.AnyAsync(p => p.Email == newUser.Email))
                return new ConflictResult<bool>(Errors.EMAIL_NOT_UNIQUE);

            if ((updateThisUser.Username != newUser.Username) && await _userRepository.AnyAsync(p => p.Username == newUser.Username))
                return new ConflictResult<bool>(Errors.USERNAME_NOT_UNIQUE);

            newUser.Email.ValidateEmail(out var errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage.Value);

            updateThisUser.Email = newUser.Email;
            updateThisUser.FirstName = newUser.FirstName;
            updateThisUser.LastName = newUser.LastName;
            updateThisUser.Username = newUser.Username;

            await _userRepository.UpdateAsync(updateThisUser);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> ChangePassword(int userId,
            string oldPassword,
            string newPassword)
        {
            User user = (await GetUsers(id: userId)).Data.FirstOrDefault();

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
