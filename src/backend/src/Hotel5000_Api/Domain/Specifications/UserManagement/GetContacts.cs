using Ardalis.Specification;
using Core.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Specifications.UserManagement
{
    public class GetContacts : Specification<Contact>
    {
        public GetContacts(int? id = null,
            int? userId = null,
            string phoneNumber = null,
            string username = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (!userId.HasValue || p.UserId == userId.Value) &&
                            (phoneNumber == null || p.MobileNumber.Contains(phoneNumber)) &&
                            (username == null || p.User.Username.Contains(phoneNumber)))
                .Include(i => i.User);
        }
    }
}
