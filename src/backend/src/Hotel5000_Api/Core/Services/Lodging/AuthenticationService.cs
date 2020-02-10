using Core.Entities.LodgingEntities;
using Core.Helpers;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Interfaces.PasswordHasher;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Lodging
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<Token> _tokenRepository;
        private readonly IAsyncRepository<Role> _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AuthenticationOptions _options;

        public AuthenticationService(IAsyncRepository<User> userRepository,
            IAsyncRepository<Token> tokenRepository,
            IAsyncRepository<Role> roleRepository,
            IPasswordHasher passwordHasher,
            ISetting<AuthenticationOptions> settings)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _options = settings.Option;
        }

        public async Task<Result<User>> AuthenticateAsync(string username, string password, string email)
        {
            //This way we don't need to implement a replica of ThenInclude from EF Core, because we cause eager loading on entities from the context.
            //Specification<User> spec = new Specification<User>()
            //    .AddInclude(p => p.Lodgings
            //        .Select(s => s.Rooms
            //            .Select(s => s.ReservationWindows)));
            //return user.WithoutPassword();

            var specification = new Specification<User>()
                .ApplyFilter(p => p.Email == email || p.Username == username)
                .AddInclude(p => p.Role);

            var user = (await _userRepository.GetAsync(specification))
                .FirstOrDefault();

            if (user == null)
                return new UnauthorizedResult<User>();
            if (!_passwordHasher.Check(user.Password, password))
                return new UnauthorizedResult<User>();

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
                return new UnauthorizedResult<User>();

            if (oldToken.UsableFrom > DateTime.Now)
                return new UnauthorizedResult<User>();
            else if (oldToken.ExpiresAt < DateTime.Now)
            {
                await _tokenRepository.DeleteAsync(oldToken);
                return new UnauthorizedResult<User>();
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

        public async Task<Result<bool>> RegisterAsync(User user, string role)
        {
            user.Email.ValidateEmail(out var errorMessage);
            user.Password.ValidatePassword(out errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage);

            user.Password = _passwordHasher.Hash(user.Password);

            var roleEntity = (await _roleRepository.GetAsync(new Specification<Role>().ApplyFilter(p => p.Name.ToString() == role))).FirstOrDefault();

            if (roleEntity == null)
                return new InvalidResult<bool>("Role not found");

            user.Role = null;

            user.RoleId = roleEntity.Id;

            await _userRepository.AddAsync(user);

            return new SuccessfulResult<bool>(true);
        }
    }
}