using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Lodging;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.LodgingDomain;
using Core.Interfaces.PasswordHasher;
using Core.Specifications;
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
            //This way we don't need to implement a replica of ThenInclude from EF Core, because we cause eager loading on entities from the context.
            //Specification<User> spec = new Specification<User>()
            //    .AddInclude(p => p.Lodgings
            //        .Select(s => s.Rooms
            //            .Select(s => s.ReservationWindows)));
            //return user.WithoutPassword();

            var specification = new Specification<User>()
                .ApplyFilter(p => p.Email == identifier || p.Username == identifier)
                .AddInclude(p => p.Role);

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

            var userWithToken = (await _userRepository.GetAsync
                    (specification.AddInclude(p => p.Tokens)))
                .FirstOrDefault();

            return new SuccessfulResult<User>(userWithToken);
        }
        public async Task<Result<bool>> LogoutAsync(string refreshToken)
        {

            var token = (await _tokenRepository.GetAsync(new Specification<Token>().ApplyFilter(p => p.RefreshToken == refreshToken))).FirstOrDefault();
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
            var oldToken = (await _tokenRepository.GetAsync(
                    new Specification<Token>()
                        .ApplyFilter(p => p.RefreshToken == refreshToken))).FirstOrDefault();

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

            var userWithToken = (await _userRepository.GetAsync(
                    new Specification<User>()
                        .ApplyFilter(p => p.Id == newToken.UserId)
                        .AddInclude(p => p.Role)
                        .AddInclude(p => p.Tokens)))
                .FirstOrDefault();

            return new SuccessfulResult<User>(userWithToken);
        }

        public async Task<Result<bool>> IsAuthorized(int resourceOwnerId, int accessingUserId)
        {
            ISpecification<User> query = new Specification<User>().AddInclude(p => p.Role);
            User resourceOwner = (await _userRepository.GetAsync(query.ApplyFilter(f => f.Id == resourceOwnerId))).FirstOrDefault();
            if (resourceOwner == null)
                return new NotFoundResult<bool>(Errors.RESOURCE_OWNER_NOT_FOUND);
            User accessingUser = (await _userRepository.GetAsync(query.ApplyFilter(f => f.Id == accessingUserId))).FirstOrDefault();
            if (accessingUser == null)
                return new NotFoundResult<bool>(Errors.ACCESSING_USER_NOT_FOUND);

            if (accessingUser.Role.Name == Roles.Admin
                || accessingUser.Id == resourceOwner.Id)
                return new SuccessfulResult<bool>(true);
            else return new UnauthorizedResult<bool>(Errors.UNAUTHORIZED);
        }
    }
}