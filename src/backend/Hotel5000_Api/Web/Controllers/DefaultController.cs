using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Enums.Logging;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Entities.LodgingEntities;
using Core.Interfaces.Lodging;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILoggingService loggingService;
        private readonly IAuthenticaton authenticatonService;
        public DefaultController(ILoggingService LoggingService, IAuthenticaton authenticaton)
        {
            loggingService = LoggingService;
            authenticatonService = authenticaton;
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await loggingService.Log("test", LogLevel.Information);

            User newUser = new User();
            newUser.RoleId = 3;
            newUser.Username = "test";
            newUser.Password = "Testpw111";
            newUser.LastName = "testlast";
            newUser.FirstName = "testfirst";
            newUser.Email = "asd@asd.com";

            await authenticatonService.RegisterAsync(newUser);

            Token token = await authenticatonService.AuthenticateAsync("test", "Testpw111", null);

            Token newToken = await authenticatonService.RefreshAsync(token.RefreshToken);
            
            return Ok();
        }
    }
}