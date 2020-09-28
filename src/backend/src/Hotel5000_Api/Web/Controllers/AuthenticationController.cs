using Auth.Options;
using AutoMapper;
using Core.Entities.Domain;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.UserManagementService;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.DTOs;
using Web.Helpers;

namespace Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authenticationService;
        private readonly IUserService _userService;
        private readonly AuthenticationOptions _options;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthentication authenticationService,
            ISetting<AuthenticationOptions> settings,
            IMapper mapper,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _options = settings.Option;
            _mapper = mapper;
            _userService = userService;
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(_options.AccessTokenDuration),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// Authenticates an user
        /// </summary>
        /// <param name="loginData">This contains the username or email of an user, and its password</param>
        /// <returns>A new JWT Token, refresh token, the user's role, username and email</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthenticationDto), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            Result<User> result =
                await _authenticationService.AuthenticateAsync(loginData.Identifier, loginData.Password);

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            var accessToken = GenerateToken(result.Data);
            var refreshToken = result.Data.Tokens.LastOrDefault().RefreshToken;
            var role = result.Data.Role.Name.ToString();
            var username = result.Data.Username;
            var email = result.Data.Email;

            return Ok(new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = role,
                Username = username,
                Email = email
            });
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(AuthenticationDto), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
        {
            Result<User> result = await _authenticationService.RefreshAsync(refreshDto.RefreshToken);

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            var accessToken = GenerateToken(result.Data);
            var newRefreshToken = result.Data.Tokens.LastOrDefault().RefreshToken;
            var role = result.Data.Role.Name.ToString();

            return Ok(new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                Role = role
            });
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Register([FromBody] UserDto newUserDto)
        {
            User newUserEntity = _mapper.Map<User>(newUserDto);

            Result<bool> result = await _userService.AddUser(newUserEntity, newUserDto.Role);

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        /// <summary>
        /// Logs out an user who has a valid refresh token
        /// </summary>
        /// <param name="refreshDto">this contains the refresh token</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST api/auth/logout
        ///     {
        ///         "refreshToken": "valid refresh token"
        ///     }
        ///     
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Logout([FromBody] RefreshDto refreshDto)
        {
            var result = await _authenticationService.LogoutAsync(refreshDto.RefreshToken);

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
    }
}