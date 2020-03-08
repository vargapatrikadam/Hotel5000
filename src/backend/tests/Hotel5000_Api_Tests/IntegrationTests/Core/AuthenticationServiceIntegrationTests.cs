using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.LodgingDomain;
using Core.Services.LodgingDomain;
using Hotel5000_Api_Tests.IntegrationTests.Core.Helpers;
using Hotel5000_Api_Tests.IntegrationTests.Core.Helpers.Authentication;
using Hotel5000_Api_Tests.UnitTests.Data;
using Xunit;

namespace Hotel5000_Api_Tests.IntegrationTests.Core
{
    public class AuthenticationServiceIntegrationTests
    {
        [Fact]
        public async Task Authenticate_InvalidUser()
        {
            var userRepo = RepositoryHelpers.GetTestUserRepository();
            var tokenRepo = RepositoryHelpers.GetTestTokenRepository();
            IAuthenticationService service = new AuthenticationService(
                userRepo,
                tokenRepo,
                AuthentiationDependencies.GetPasswordHasher(),
                AuthentiationDependencies.GetAuthenticationOptions());

            var result = await service.AuthenticateAsync("non existent user", "non existent user password");

            Assert.IsType<UnauthorizedResult<User>>(result);
        } 
        [Fact]
        public async Task Authenticate_BadPassword()
        {
            var userRepo = RepositoryHelpers.GetTestUserRepository();
            var testUser = AuthenticationEntities.GetTestUser();
            var tokenRepo = RepositoryHelpers.GetTestTokenRepository();
            var passwordHasher = AuthentiationDependencies.GetPasswordHasher();

            testUser.Password = passwordHasher.Hash(testUser.Password);
            await userRepo.AddAsync(testUser);

            IAuthenticationService service = new AuthenticationService(
                userRepo,
                tokenRepo,
                passwordHasher,
                AuthentiationDependencies.GetAuthenticationOptions());

            var result = await service.AuthenticateAsync("testusername", "badpassword");

            Assert.IsType<UnauthorizedResult<User>>(result);
        }
        [Fact]
        public async Task Authenticate_Successful()
        {
            var userRepo = RepositoryHelpers.GetTestUserRepository();
            var testUser = AuthenticationEntities.GetTestUser();
            var tokenRepo = RepositoryHelpers.GetTestTokenRepository();
            var passwordHasher = AuthentiationDependencies.GetPasswordHasher();

            testUser.Password = passwordHasher.Hash(testUser.Password);
            await userRepo.AddAsync(testUser);

            IAuthenticationService service = new AuthenticationService(
                userRepo,
                tokenRepo,
                passwordHasher,
                AuthentiationDependencies.GetAuthenticationOptions());

            var result = await service.AuthenticateAsync("testusername", "testpassword");

            Assert.IsType<SuccessfulResult<User>>(result);
        }
    }
}
