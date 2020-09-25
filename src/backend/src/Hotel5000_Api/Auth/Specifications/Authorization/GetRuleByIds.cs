using Ardalis.Specification;
using Core.Entities.Authentication;

namespace Auth.Specifications.Authorization
{
    class GetRuleByIds : Specification<Rule>
    {
        public GetRuleByIds(int roleId, int operationId, int entityId)
        {
            Query
                .Where(p => p.Role_Id == roleId &&
                            p.Operation_Id == operationId &&
                            p.Entity_Id == entityId);
        }
    }
}
