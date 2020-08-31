using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.Specifications.Authentication
{
    public class GetUserByIdWithRoleAndToken : Specification<User>
    {
        public GetUserByIdWithRoleAndToken(int userId)
        {
            Query
                .Where(p => p.Id == userId)
                .Include(i => i.Role);

            Query.Include(i => i.Tokens);
        }
    }
}
