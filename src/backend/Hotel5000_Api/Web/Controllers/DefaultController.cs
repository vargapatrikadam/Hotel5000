using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Enums.Logging;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILoggingService loggingService;
        public DefaultController(ILoggingService LoggingService)
        {
            loggingService = LoggingService;
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            loggingService.Log("test", LogLevel.Information);
            return Ok();
        }
    }
}