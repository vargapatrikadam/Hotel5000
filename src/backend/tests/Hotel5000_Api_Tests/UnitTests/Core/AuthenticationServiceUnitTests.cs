using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Interfaces.Lodging.UserManagementService;
using Core.Interfaces.PasswordHasher;
using Core.Services.Lodging;
using Core.Specifications;
using Hotel5000_Api_Tests.UnitTests.Core.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hotel5000_Api_Tests.UnitTests.Core
{
    public class AuthenticationServiceUnitTests
    {
        [Fact]
        public async Task Authenticate_InvalidUser()
        {
            var mockUserRepo = new Mock<IAsyncRepository<User>>();
            mockUserRepo.Setup(s => s.GetAsync(It.IsAny<ISpecification<User>>()))
                .ReturnsAsync(new List<User>());
            var mockTokenRepo = new Mock<IAsyncRepository<Token>>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var mockAuthenticationOptions = new Mock<ISetting<AuthenticationOptions>>();
            var mockUserService = new Mock<IUserService>();
            mockAuthenticationOptions.Setup(s => s.Option)
                .Returns(new AuthenticationOptions()
                {
                    AccessTokenDuration = 60,
                    RefreshTokenDuration = 60,
                    Secret = "Unit Test Secret"
                });
            var service = new AuthenticationService(mockUserRepo.Object, mockTokenRepo.Object, mockPasswordHasher.Object, mockAuthenticationOptions.Object, mockUserService.Object);

            var result = await service.AuthenticateAsync("asd", "asd", "asd");

            Assert.IsType<UnauthorizedResult<User>>(result);
        }

        [Fact]
        public async Task Authenticate_BadPassword()
        {
            var mockUserRepo = new Mock<IAsyncRepository<User>>();
            mockUserRepo.Setup(s => s.GetAsync(It.IsAny<ISpecification<User>>()))
                .ReturnsAsync(new List<User>() { AuthenticationEntities.GetTestUser() });
            var mockTokenRepo = new Mock<IAsyncRepository<Token>>();
            var mockUserService = new Mock<IUserService>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            mockPasswordHasher.Setup(s => s.Check(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            var mockAuthenticationOptions = new Mock<ISetting<AuthenticationOptions>>();
            mockAuthenticationOptions.Setup(s => s.Option)
                .Returns(new AuthenticationOptions()
                {
                    AccessTokenDuration = 60,
                    RefreshTokenDuration = 60,
                    Secret = "Unit Test Secret"
                });
            var service = new AuthenticationService(mockUserRepo.Object, mockTokenRepo.Object, mockPasswordHasher.Object, mockAuthenticationOptions.Object, mockUserService.Object);

            var result = await service.AuthenticateAsync("testusername", "badpassword", "testemail");

            Assert.IsType<UnauthorizedResult<User>>(result);
        }
        [Fact]
        public async Task Authenticate_Successful()
        {
            var mockUserRepo = new Mock<IAsyncRepository<User>>();
            mockUserRepo.Setup(s => s.GetAsync(It.IsAny<ISpecification<User>>()))
                .ReturnsAsync(new List<User>() { AuthenticationEntities.GetTestUser() });
            var mockTokenRepo = new Mock<IAsyncRepository<Token>>();
            Token insertedToken = null;
            mockTokenRepo.Setup(s => s.AddAsync(It.IsAny<Token>()))
                .Callback((Token t) => insertedToken = t);
            var mockUserService = new Mock<IUserService>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            mockPasswordHasher.Setup(s => s.Check(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var mockAuthenticationOptions = new Mock<ISetting<AuthenticationOptions>>();
            mockAuthenticationOptions.Setup(s => s.Option)
                .Returns(new AuthenticationOptions()
                {
                    AccessTokenDuration = 60,
                    RefreshTokenDuration = 60,
                    Secret = "Unit Test Secret"
                });
            var service = new AuthenticationService(mockUserRepo.Object, mockTokenRepo.Object, mockPasswordHasher.Object, mockAuthenticationOptions.Object, mockUserService.Object);

            var result = await service.AuthenticateAsync("testusername", "testpassword", "testemail");

            Assert.IsType<SuccessfulResult<User>>(result);
            Assert.True(insertedToken.UserId == 1);
        }
    }
}
