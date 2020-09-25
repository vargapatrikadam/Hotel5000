using Ardalis.Specification;
using Core.Entities.Domain;

namespace Auth.Specifications.Authentication
{
    class GetTokenByRefreshToken : Specification<Token>
    {
        public GetTokenByRefreshToken(string refreshToken)
        {
            Query
                .Where(p => p.RefreshToken == refreshToken);
        }
    }
}
