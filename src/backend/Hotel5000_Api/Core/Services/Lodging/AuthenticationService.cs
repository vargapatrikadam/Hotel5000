using Core.Entities.LodgingEntities;
using Core.Helpers;
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
        private readonly IAsyncRepository<User> UserRepository;
        private readonly IAsyncRepository<Token> TokenRepository;
        private readonly IPasswordHasher PasswordHasher;
        private readonly AuthenticationOptions Options;
        public AuthenticationService(IAsyncRepository<User> userRepository, 
                                    IAsyncRepository<Token> tokenRepository,
                                    IPasswordHasher passwordHasher,
                                    IOption<AuthenticationOptions> options)
        {
            UserRepository = userRepository;
            TokenRepository = tokenRepository;
            PasswordHasher = passwordHasher;
            Options = options.option;
        }
        public async Task<User> AuthenticateAsync(string username, string password, string email)
        {
            //This way we don't need to implement a replica of ThenInclude from EF Core, because we cause eager loading on entities from the context.
            //Specification<User> spec = new Specification<User>()
            //    .AddInclude(p => p.Lodgings
            //        .Select(s => s.Rooms
            //            .Select(s => s.ReservationWindows)));
            //return user.WithoutPassword();

            User user = (await UserRepository.GetAsync(
                new Specification<User>()
                    .ApplyFilter(p => p.Email == email || p.Username == username)
                    .AddInclude(p => p.Role)))
                    .FirstOrDefault();

            if (user == null)
                return null;
            if (!PasswordHasher.Check(user.Password, password))
                return null;

            Token newToken = new Token();
            newToken.RefreshToken = GenerateRefreshToken();
            newToken.UserId = user.Id;
            newToken.UsableFrom = DateTime.Now.AddSeconds(Options.AccessTokenDuration);
            newToken.ExpiresAt = newToken.UsableFrom.AddSeconds(Options.RefreshTokenDuration);

            await TokenRepository.AddAsync(newToken);

            User userWithToken = (await UserRepository.GetAsync(
                new Specification<User>()
                    .ApplyFilter(p => p.Email == email || p.Username == username)
                    .AddInclude(p => p.Role)
                    .AddInclude(p => p.Tokens)))
                    .FirstOrDefault();

            return userWithToken;
        }
        private string GenerateRefreshToken()
        {
            byte[] random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public async Task<User> RefreshAsync(string refreshToken)
        {
            Token oldToken
                = (await TokenRepository.GetAsync(
                new Specification<Token>()
                .ApplyFilter(p => p.RefreshToken == refreshToken)
                .AddInclude(p => p.User.Role)))
                .FirstOrDefault();

            if (oldToken == null)
                return null;

            if (oldToken.UsableFrom > DateTime.Now)
                return null;
            else if (oldToken.ExpiresAt < DateTime.Now)
                return null;

            Token newToken = new Token();
            newToken.RefreshToken = GenerateRefreshToken();
            newToken.UserId = oldToken.UserId;
            newToken.UsableFrom = DateTime.Now.AddSeconds(Options.AccessTokenDuration);
            newToken.ExpiresAt = newToken.UsableFrom.AddSeconds(Options.RefreshTokenDuration);

            await TokenRepository.AddAsync(newToken);

            await TokenRepository.DeleteAsync(oldToken);

            User userWithToken = (await UserRepository.GetAsync(
                new Specification<User>()
                    .ApplyFilter(p => p.Id == newToken.UserId)
                    .AddInclude(p => p.Role)
                    .AddInclude(p => p.Tokens)))
                    .FirstOrDefault();

            return userWithToken;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            string errorMessage = null;
            user.Email.ValidateEmail(out errorMessage);
            user.Password.ValidatePassword(out errorMessage);
            if (errorMessage != string.Empty)
                throw new ArgumentException(errorMessage);

            user.Password = PasswordHasher.Hash(user.Password);

            await UserRepository.AddAsync(user);

            return true;
        }
    }
}
