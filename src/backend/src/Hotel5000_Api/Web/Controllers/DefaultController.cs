using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Enums.Logging;
using Infrastructure.Lodgings;
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
        private readonly ILoggingService _loggingService;
        private readonly IAuthenticationService _authenticatonService;

        public DefaultController(ILoggingService loggingService, IAuthenticationService authenticaton)
        {
            _loggingService = loggingService;
            _authenticatonService = authenticaton;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await _loggingService.Log("test", LogLevel.Information);

            var data = new List<string>()
            {
                "test1",
                "test2",
                "test3"
            };
            return Ok(data);
        }

        [AuthorizeRoles(Roles.Admin, Roles.ApprovedUser, Roles.Company)]
        [HttpGet("testAuthenticate")]
        public async Task<IActionResult> TestAuthenticate()
        {
            return Ok();
        }
    }
}