using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.UserManagementService;
using Core.Results;
using Domain.Specifications.UserManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.UserManagement
{
    class ContactService : IContactService
    {
        private readonly IAsyncRepository<Contact> _contactRepository;
        private readonly IAuthorization _authorizationService;
        private readonly IUserService _userService;
        public ContactService(IAsyncRepository<Contact> contactRepository, IAuthorization authorizationService, IUserService userService)
        {
            _contactRepository = contactRepository;
            _authorizationService = authorizationService;
            _userService = userService;
        }
        public async Task<Result<bool>> AddContact(Contact contact)
        {
            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Contact)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(contact.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (await _contactRepository.AnyAsync(p => p.MobileNumber == contact.MobileNumber))
                return new ConflictResult<bool>(Errors.CONTACT_NOT_UNIQUE);

            await _contactRepository.AddAsync(contact);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<IReadOnlyList<Contact>>> GetContacts(int? id = null,
            int? userId = null,
            string phoneNumber = null,
            string username = null)
        {
            var specification = new GetContacts(id, userId, phoneNumber, username);

            return new SuccessfulResult<IReadOnlyList<Contact>>(await _contactRepository.GetAsync(specification));
        }

        public async Task<Result<bool>> RemoveContact(int contactOwnerId, int contactId)
        {
            User user = (await _userService.GetUsers(id: contactOwnerId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Contact)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(user.Id));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            Contact contact = user.Contacts.Where(p => p.Id == contactId).FirstOrDefault();

            if (contact == null)
                return new NotFoundResult<bool>(Errors.CONTACT_NOT_FOUND);

            await _contactRepository.DeleteAsync(contact);
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
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

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
    }
}
