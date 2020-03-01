﻿using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain
{
    public interface IAuthenticationService
    {
        Task<Result<User>> AuthenticateAsync(string username, string password, string email);
        Task<Result<User>> RefreshAsync(string refreshToken);
        Task<Result<bool>> IsAuthorized(int resourceOwnerId, int accessingUserId);
    }
}