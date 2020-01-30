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
using Web.Attributes;
using Core.Enums.Lodging;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILoggingService loggingService;
        private readonly IAuthenticationService authenticatonService;
        public DefaultController(ILoggingService LoggingService, IAuthenticationService authenticaton)
        {
            loggingService = LoggingService;
            authenticatonService = authenticaton;
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await loggingService.Log("test", LogLevel.Information);

            List<string> data = new List<string>()
            {
                "test1",
                "test2",
                "test3"
            };
            return Ok(data);
        }

        [AuthorizeRoles(Roles.ADMIN, Roles.APPROVED_USER, Roles.COMPANY)]
        [HttpGet("testAuthenticate")]
        public async Task<IActionResult> TestAuthenticate()
        {
            return Ok();
        }
    }
}