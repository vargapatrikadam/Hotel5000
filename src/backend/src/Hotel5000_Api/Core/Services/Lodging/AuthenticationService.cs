﻿using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using Core.Helpers;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Interfaces.Lodging.UserManagementService;
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
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AuthenticationOptions _options;

        public AuthenticationService(IAsyncRepository<User> userRepository,
            IAsyncRepository<Token> tokenRepository,
            IPasswordHasher passwordHasher,
            ISetting<AuthenticationOptions> settings,
            IUserService userService)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordHasher = passwordHasher;
            _options = settings.Option;
            _userService = userService;
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
            return await _userService.AddUser(user, role);
        }
    }
}