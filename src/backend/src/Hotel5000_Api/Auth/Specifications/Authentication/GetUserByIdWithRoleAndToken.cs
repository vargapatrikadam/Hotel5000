using Ardalis.Specification;
using Core.Entities.Domain;

namespace Auth.Specifications.Authentication
{
    class GetUserByIdWithRoleAndToken : Specification<User>
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
