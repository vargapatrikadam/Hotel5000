using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using Core.Enums.Authentication;
using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.UserManagement
{
    public class GetRole : Specification<Role>
    {
        public GetRole(RoleType role)
        {
            Query
                .Where(p => p.Name == role);
        }
    }
}
