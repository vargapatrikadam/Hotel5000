using Ardalis.Specification;
using Core.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Text;

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
