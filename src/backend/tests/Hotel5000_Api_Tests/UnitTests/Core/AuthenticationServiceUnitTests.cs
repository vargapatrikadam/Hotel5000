using Ardalis.Specification;
using Auth.Identity;
using Core.Entities.Domain;
using Core.Results;
using Core.Interfaces;
using Core.Interfaces.PasswordHasher;
using Hotel5000_Api_Tests.UnitTests.Data;
using Hotel5000_Api_Tests.UnitTests.Helpers;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Auth.Options;
using Auth.Services.Authentication;

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
            var service = new AuthenticationService(mockUserRepo.Object, mockTokenRepo.Object, mockPasswordHasher.Object, mockAuthenticationOptions.Object);

            var result = await service.AuthenticateAsync("asd", "asd");

            Assert.IsType<UnauthorizedResult<User>>(result);
        }

        [Fact]
        public async Task Authenticate_BadPassword()
        {
            var mockUserRepo = new Mock<IAsyncRepository<User>>();
            mockUserRepo.Setup(s => s.GetAsync(It.IsAny<ISpecification<User>>()))
                .ReturnsAsync(new List<User>() { AuthenticationEntities.GetTestUser() });
            var mockTokenRepo = new Mock<IAsyncRepository<Token>>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            mockPasswordHasher.Setup(s => s.Check(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            var mockAuthenticationOptions = new Mock<ISetting<AuthenticationOptions>>();
            mockAuthenticationOptions.Setup(s => s.Option)
                .Returns(AuthenticationHelpers.GetTestAuthenticationOptions());
            var service = new AuthenticationService(mockUserRepo.Object, mockTokenRepo.Object, mockPasswordHasher.Object, mockAuthenticationOptions.Object);

            var result = await service.AuthenticateAsync("testusername", "badpassword");

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
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            mockPasswordHasher.Setup(s => s.Check(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var mockAuthenticationOptions = new Mock<ISetting<AuthenticationOptions>>();
            mockAuthenticationOptions.Setup(s => s.Option)
                .Returns(AuthenticationHelpers.GetTestAuthenticationOptions());
            var service = new AuthenticationService(mockUserRepo.Object, mockTokenRepo.Object, mockPasswordHasher.Object, mockAuthenticationOptions.Object);

            var result = await service.AuthenticateAsync("testemail", "testpassword");

            Assert.IsType<SuccessfulResult<User>>(result);
            Assert.True(insertedToken.UserId == 1);
        }
    }
}
