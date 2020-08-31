using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Users
{
    public class GetRole : Specification<Role>
    {
        public GetRole(Roles role)
        {
            Query
                .Where(p => p.Name == role);
        }
    }
}
