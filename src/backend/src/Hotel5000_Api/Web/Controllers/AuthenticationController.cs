using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.LodgingEntities;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly AuthenticationOptions _options;

        public AuthenticationController(IAuthenticationService authenticationService,
            ISetting<AuthenticationOptions> settings)
        {
            _authenticationService = authenticationService;
            _options = settings.Option;
        }

        private string GenerateToken(string secret, int expiration, User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.UserData, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(expiration),
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
            var authenticatedUser =
                await _authenticationService.AuthenticateAsync(loginData.Username, loginData.Password, loginData.Email);
            if (authenticatedUser == null)
                return BadRequest(new ErrorDto {Message = "Invalid login data"});
            var accessToken = GenerateToken(_options.Secret, _options.AccessTokenDuration, authenticatedUser);
            var refreshToken = authenticatedUser.Tokens.FirstOrDefault().RefreshToken;
            return Ok(new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(AuthenticationDto), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var authenticatedUser = await _authenticationService.RefreshAsync(refreshToken);
            if (authenticatedUser == null)
                return BadRequest(new ErrorDto {Message = "Invalid refresh token"});
            var accessToken = GenerateToken(_options.Secret, _options.AccessTokenDuration, authenticatedUser);
            var newRefreshToken = authenticatedUser.Tokens.FirstOrDefault().RefreshToken;
            return Ok(new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}