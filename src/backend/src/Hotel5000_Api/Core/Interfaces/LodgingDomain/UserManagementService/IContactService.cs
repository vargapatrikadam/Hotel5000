﻿using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain.UserManagementService
{
    public interface IContactService
    {
        Task<Result<bool>> AddContact(Contact contact);
        Task<Result<bool>> RemoveContact(int contactOwnerId, int contactId);
        Task<Result<bool>> UpdateContact(Contact newContact, int contactId);
        Task<Result<IReadOnlyList<Contact>>> GetContacts(int? id = null, int? userId = null, string phoneNumber = null, string username = null);
    }
}
