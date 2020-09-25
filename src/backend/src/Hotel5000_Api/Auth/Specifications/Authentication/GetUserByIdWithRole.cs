using Ardalis.Specification;
using Core.Entities.Domain;

namespace Auth.Specifications.Authentication
{
    class GetUserByIdWithRole : Specification<User>
    {
        public GetUserByIdWithRole(int userId)
        {
            Query
                .Where(p => p.Id == userId)
                .Include(i => i.Role);
        }
    }
}
