using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Services.Lodging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web.DTOs;

namespace Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly AuthenticationOptions _options;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authenticationService,
            ISetting<AuthenticationOptions> settings,
            IMapper mapper)
        {
            _authenticationService = authenticationService;
            _options = settings.Option;
            _mapper = mapper;
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.UserData, user.Id.ToString()),
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

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthenticationDto), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            Result<User> result =
                await _authenticationService.AuthenticateAsync(loginData.Username, loginData.Password, loginData.Email);

            if (result.ResultType == ResultType.Unauthorized)
                return BadRequest(new ErrorDto("Invalid login data"));

            var accessToken = GenerateToken(result.Data);
            var refreshToken = result.Data.Tokens.LastOrDefault().RefreshToken;
            var role = result.Data.Role.Name.ToString();

            return Ok(new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = role
            });
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(AuthenticationDto), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            Result<User> result = await _authenticationService.RefreshAsync(refreshToken);
            if (result.ResultType == ResultType.Unauthorized)
                return BadRequest(new ErrorDto("Invalid refresh token"));
            var accessToken = GenerateToken(result.Data);
            var newRefreshToken = result.Data.Tokens.FirstOrDefault().RefreshToken;
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
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            User userEntity = _mapper.Map<User>(newUser);

            Result<bool> result = await _authenticationService.RegisterAsync(userEntity);

            if (result.Data == true)
                return Ok();
            else return BadRequest(new ErrorDto(result.Errors));
        }
    }
}