using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Lodging;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.LodgingDomain;
using Core.Interfaces.PasswordHasher;
using Core.Specifications;
using Core.Specifications.Authentication;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Core.Services.LodgingDomain
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<Token> _tokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AuthenticationOptions _options;

        public AuthenticationService(IAsyncRepository<User> userRepository,
            IAsyncRepository<Token> tokenRepository,
            IPasswordHasher passwordHasher,
            ISetting<AuthenticationOptions> settings)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordHasher = passwordHasher;
            _options = settings.Option;
        }

        public async Task<Result<User>> AuthenticateAsync(string identifier, string password)
        {
            var specification = new GetUserByIdentifier(identifier);

            var user = (await _userRepository.GetAsync(specification))
                .FirstOrDefault();

            if (user == null)
                return new UnauthorizedResult<User>(Errors.USER_NOT_FOUND);
            if (!_passwordHasher.Check(user.Password, password))
                return new UnauthorizedResult<User>(Errors.PASSWORD_INCORRECT);

            var newToken = new Token
            {
                RefreshToken = GenerateRefreshToken(),
                UserId = user.Id,
                UsableFrom = DateTime.Now.AddSeconds(_options.AccessTokenDuration)
            };
            newToken.ExpiresAt = newToken.UsableFrom.AddSeconds(_options.RefreshTokenDuration);

            await _tokenRepository.AddAsync(newToken);

            specification = new GetUserByIdentifier(identifier, true);
            
            var userWithToken = (await _userRepository.GetAsync
                    (specification))
                .FirstOrDefault();

            return new SuccessfulResult<User>(userWithToken);
        }
        public async Task<Result<bool>> LogoutAsync(string refreshToken)
        {
            var specification = new GetTokenByRefreshToken(refreshToken);

            var token = (await _tokenRepository.GetAsync(specification)).FirstOrDefault();
            if (token == null)
                return new NotFoundResult<bool>(Errors.TOKEN_NOT_FOUND);

            await _tokenRepository.DeleteAsync(token);

            return new SuccessfulResult<bool>(true);
        }


        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<Result<User>> RefreshAsync(string refreshToken)
        {
            var specification = new GetTokenByRefreshToken(refreshToken);

            var oldToken = (await _tokenRepository.GetAsync(specification)).FirstOrDefault();

            if (oldToken == null)
                return new UnauthorizedResult<User>(Errors.TOKEN_NOT_FOUND);

            if (oldToken.UsableFrom > DateTime.Now)
                return new UnauthorizedResult<User>(Errors.TOKEN_INVALID);
            else if (oldToken.ExpiresAt < DateTime.Now)
            {
                await _tokenRepository.DeleteAsync(oldToken);
                return new UnauthorizedResult<User>(Errors.TOKEN_INVALID);
            }


            var newToken = new Token
            {
                RefreshToken = GenerateRefreshToken(),
                UserId = oldToken.UserId,
                UsableFrom = DateTime.Now.AddSeconds(_options.AccessTokenDuration)
            };
            newToken.ExpiresAt = newToken.UsableFrom.AddSeconds(_options.RefreshTokenDuration);

            await _tokenRepository.AddAsync(newToken);

            await _tokenRepository.DeleteAsync(oldToken);

            var getUserSpecification = new GetUserByIdWithRoleAndToken(newToken.UserId);

            var userWithToken = (await _userRepository.GetAsync(getUserSpecification))
                .FirstOrDefault();

            return new SuccessfulResult<User>(userWithToken);
        }

        public async Task<Result<bool>> IsAuthorized(int resourceOwnerId, int accessingUserId)
        {
            var getResourceOwnerSpecification = new GetUserByIdWithRole(resourceOwnerId);
            User resourceOwner = (await _userRepository.GetAsync(getResourceOwnerSpecification)).FirstOrDefault();
            if (resourceOwner == null)
                return new NotFoundResult<bool>(Errors.RESOURCE_OWNER_NOT_FOUND);

            var getAccessingUserSpecification = new GetUserByIdWithRole(accessingUserId);
            User accessingUser = (await _userRepository.GetAsync(getAccessingUserSpecification)).FirstOrDefault();
            if (accessingUser == null)
                return new NotFoundResult<bool>(Errors.ACCESSING_USER_NOT_FOUND);

            if (accessingUser.Role.Name == Roles.Admin
                || accessingUser.Id == resourceOwner.Id)
                return new SuccessfulResult<bool>(true);
            else return new UnauthorizedResult<bool>(Errors.UNAUTHORIZED);
        }
    }
}