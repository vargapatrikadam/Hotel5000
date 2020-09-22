using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Implementations;
using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Authentication;
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
            string dbName = "invalid";
            var userRepo = RepositoryHelpers.GetTestUserRepository(dbName);
            var tokenRepo = RepositoryHelpers.GetTestTokenRepository(dbName);
            IAuthentication service = new AuthenticationService(
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
            string dbName = "badpassword";
            var userRepo = RepositoryHelpers.GetTestUserRepository(dbName);
            var testUser = AuthenticationEntities.GetTestUser();
            var tokenRepo = RepositoryHelpers.GetTestTokenRepository(dbName);
            var passwordHasher = AuthentiationDependencies.GetPasswordHasher();

            testUser.Password = passwordHasher.Hash(testUser.Password);
            await userRepo.AddAsync(testUser);

            IAuthentication service = new AuthenticationService(
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
            string dbName = "successful";
            var userRepo = RepositoryHelpers.GetTestUserRepository(dbName);
            var testUser = AuthenticationEntities.GetTestUser();
            var tokenRepo = RepositoryHelpers.GetTestTokenRepository(dbName);
            var passwordHasher = AuthentiationDependencies.GetPasswordHasher();
            var authenticationOptions = AuthentiationDependencies.GetAuthenticationOptions();

            testUser.Password = passwordHasher.Hash(testUser.Password);
            await userRepo.AddAsync(testUser);

            IAuthentication service = new AuthenticationService(
                userRepo,
                tokenRepo,
                passwordHasher,
                authenticationOptions);

            var result = await service.AuthenticateAsync("testusername", "testpassword");

            Assert.IsType<SuccessfulResult<User>>(result);
        }
    }
}
