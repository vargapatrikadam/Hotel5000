using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Authentication
{
    public class GetTokenByRefreshToken : Specification<Token>
    {
        public GetTokenByRefreshToken(string refreshToken)
        {
            Query
                .Where(p => p.RefreshToken == refreshToken);
        }
    }
}
