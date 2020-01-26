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

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILoggingService loggingService;
        private readonly IAsyncRepository<Role> repository;
        public DefaultController(ILoggingService LoggingService, IAsyncRepository<Role> repository)
        {
            loggingService = LoggingService;
            this.repository = repository;
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await loggingService.Log("test", LogLevel.Information);

            IReadOnlyList<Role> roles = await repository.GetAllAsync();
            
            return Ok();
        }
    }
}