using Ardalis.Specification;
using Core.Entities.Authentication;
using Core.Enums.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Specifications.Authorization
{
    class GetRoleByName : Specification<BaseRole>
    {
        public GetRoleByName(RoleType nameAsType)
        {
            Query
                .Where(p => p.Name == nameAsType);
        }
    }
}
