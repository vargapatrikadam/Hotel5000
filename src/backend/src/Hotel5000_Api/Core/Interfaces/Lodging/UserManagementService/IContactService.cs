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
        Task<Result<bool>> AddContact(Contact contact, int userId);
        Task<Result<bool>> RemoveContact(int contactId);
        Task<Result<bool>> UpdateContact(Contact contact, int userId, int contactId);
        Task<Result<IReadOnlyList<Contact>>> GetContacts(int userId);
        Task<Result<IReadOnlyList<Contact>>> GetAllContacts();
    }
}
