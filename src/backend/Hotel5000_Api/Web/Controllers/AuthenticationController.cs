﻿using System;
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

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly AuthenticationOptions options;
        public AuthenticationController(IAuthenticationService authenticationService, IOption<AuthenticationOptions> options)
        {
            AuthenticationService = authenticationService;
            this.options = options.option;
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
        public async Task<IActionResult> Login([FromBody] User loginData)
        {
            User authenticatedUser = await AuthenticationService.AuthenticateAsync(loginData.Username, loginData.Password, loginData.Email);
            if (authenticatedUser == null)
                return BadRequest(new { Error = "Invalid login data" });
            string accessToken = GenerateToken(options.Secret, options.AccessTokenDuration, authenticatedUser);
            string refreshToken = authenticatedUser.Tokens.FirstOrDefault().RefreshToken;
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        [AllowAnonymous]
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            User authenticatedUser = await AuthenticationService.RefreshAsync(refreshToken);
            if (authenticatedUser == null)
                return BadRequest(new { Error = "Invalid refresh token" });
            string accessToken = GenerateToken(options.Secret, options.AccessTokenDuration, authenticatedUser);
            string newRefreshToken = authenticatedUser.Tokens.FirstOrDefault().RefreshToken;
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}