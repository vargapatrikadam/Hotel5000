using Ardalis.Specification;
using Core.Entities.Authentication;

namespace Auth.Specifications.Authorization
{
    class GetEntityByName : Specification<Entity>
    {
        public GetEntityByName(string name)
        {
            Query
                .Where(p => p.Name == name);
        }
    }
}
