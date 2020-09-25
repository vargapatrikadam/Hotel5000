﻿using Ardalis.Specification;
using Core.Entities.Domain;

namespace Domain.Specifications.UserManagement
{
    public class GetUser : Specification<User>
    {
        public GetUser(int? id = null,
            string username = null,
            string email = null,
            int? skip = null,
            int? take = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id) &&
                            (username == null || p.Username.Contains(username)) &&
                            (email == null || p.Email.Contains(email)));
            Query.Include(i => i.Role);
            Query.Include(i => i.Contacts);
            Query.Include(i => i.ApprovingData);

            if (skip.HasValue && take.HasValue)
                Query.Paginate(skip.Value, take.Value);
        }
    }
}
