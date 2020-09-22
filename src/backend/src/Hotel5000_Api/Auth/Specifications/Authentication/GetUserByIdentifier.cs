using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Specifications.Authentication
{
    class GetUserByIdentifier : Specification<User>
    {
        public GetUserByIdentifier(string identifier, bool getTokens = false)
        {
            Query
                .Where(p => p.Email == identifier || p.Username == identifier)
                .Include(i => i.Role);

            if (getTokens)
                Query.Include(i => i.Tokens);
        }
    }
}
