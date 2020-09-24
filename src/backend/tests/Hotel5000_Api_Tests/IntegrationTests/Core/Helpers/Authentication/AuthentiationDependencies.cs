using Auth.Options;
using Core.Helpers;
using Core.Helpers.PasswordHasher;
using Core.Interfaces;
using Core.Interfaces.PasswordHasher;
using Domain.Services.PasswordHasher;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel5000_Api_Tests.IntegrationTests.Core.Helpers.Authentication
{
    public static class AuthentiationDependencies
    {
        public static ISetting<HashingOptions> GetHashingOptions()
        {
            return new Setting<HashingOptions>(new HashingOptions
            {
                Iterations = 10000,
                KeySize = 32,
                SaltSize = 16
            });
        }
        public static IPasswordHasher GetPasswordHasher() 
        {
            return new PasswordHasher(GetHashingOptions());
        }
        public static ISetting<AuthenticationOptions> GetAuthenticationOptions()
        {
            return new Setting<AuthenticationOptions>(new AuthenticationOptions
            {
                AccessTokenDuration = 60,
                RefreshTokenDuration = 60,
                Secret = "Testing"
            });
        }
    }
}
