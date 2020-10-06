using Ardalis.Specification;
using Core.Entities.Authentication;

namespace Auth.Specifications.Authorization
{
    class GetRuleByIds : Specification<Rule>
    {
        public GetRuleByIds(int roleId, int operationId, int entityId)
        {
            Query
                .Where(p => p.RoleId == roleId &&
                            p.OperationId == operationId &&
                            p.EntityId == entityId);
        }
    }
}
