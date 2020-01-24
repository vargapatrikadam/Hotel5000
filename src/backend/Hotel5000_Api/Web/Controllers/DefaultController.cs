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
using Core.Entities.Example;
using Web.DTOs;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILoggingService loggingService;
        private readonly IAsyncRepository<ExampleEntity> repository;
        public DefaultController(ILoggingService LoggingService, IAsyncRepository<ExampleEntity> repository)
        {
            loggingService = LoggingService;
            this.repository = repository;
        }
        [HttpGet("test")]
        public async Task<IActionResult> Test(UserDTO user)
        {
            await loggingService.Log("test", LogLevel.Information);
            ExampleEntity exampleEntity = new ExampleEntity();
            exampleEntity.Added = DateTime.Now;
            exampleEntity.Sum = 20;
            ExampleEntity exampleEntity2 = new ExampleEntity();
            exampleEntity2.Added = DateTime.Now;
            exampleEntity2.Sum = 40;
            await repository.AddAsync(exampleEntity);
            await repository.AddAsync(exampleEntity2);
            IReadOnlyList<ExampleEntity> exampleEntities = await repository.GetAllAsync();
            await repository.DeleteAsync(exampleEntity);
            ExampleEntity deletedentity = new ExampleEntity();
            deletedentity.Sum = 20;
            deletedentity.Added = DateTime.Now;
            await repository.AddAsync(deletedentity);
            IReadOnlyCollection<ExampleEntity> exampleEntities1 = await repository.GetAllAsync();
            return Ok();
        }
    }
}