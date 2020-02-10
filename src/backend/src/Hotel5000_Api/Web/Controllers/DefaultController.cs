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
using AutoMapper;
using Core.Specifications;
using Web.DTOs;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILoggingService _loggingService;
        private readonly IAuthenticationService _authenticatonService;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<User> _asyncRepository;

        public DefaultController(ILoggingService loggingService, IAuthenticationService authenticaton, IMapper mapper, IAsyncRepository<User> asyncRepository)
        {
            _asyncRepository = asyncRepository;
            _mapper = mapper;
            _loggingService = loggingService;
            _authenticatonService = authenticaton;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            //await _loggingService.Log("test", LogLevel.Information);
            ////throw new Exception("asd");
            //var data = new List<string>()
            //{
            //    "test1",
            //    "test2",
            //    "test3"
            //};
            ICollection<User> users = (await _asyncRepository.GetAsync(new Specification<User>().AddInclude(p => p.Role))).ToList();
            ICollection<UserDto> mappedUsers = _mapper.Map<ICollection<UserDto>>(users);
            return Ok(mappedUsers);
        }

        [AuthorizeRoles(Roles.Admin, Roles.ApprovedUser, Roles.Company)]
        [HttpGet("testAuthenticate")]
        public async Task<IActionResult> TestAuthenticate()
        {
            return Ok();
        }
    }
}