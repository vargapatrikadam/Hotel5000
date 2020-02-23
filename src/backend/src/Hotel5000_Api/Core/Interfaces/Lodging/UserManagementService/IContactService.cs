using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Lodging.UserManagementService
{
    public interface IContactService
    {
        Task<Result<bool>> AddContact(Contact contact, int resourceAccessorId);
        Task<Result<bool>> RemoveContact(int contactOwnerId, int contactId, int resourceAccessorId);
        Task<Result<bool>> UpdateContact(Contact newContact, int oldContactId, int resourceAccessorId);
        Task<Result<IReadOnlyList<Contact>>> GetContacts(int userId);
        Task<Result<IReadOnlyList<Contact>>> GetAllContacts();
    }
}
