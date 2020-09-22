﻿using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Authentication
{
    public interface IAuthentication
    {
        Task<Result<User>> AuthenticateAsync(string identifier, string password);
        Task<Result<bool>> LogoutAsync(string refreshToken);
        Task<Result<User>> RefreshAsync(string refreshToken);
    }
}