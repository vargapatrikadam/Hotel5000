using Ardalis.Specification;
using Core.Entities.Authentication;
using Core.Enums.Authentication;

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
