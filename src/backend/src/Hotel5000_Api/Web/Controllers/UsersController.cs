using AutoMapper;
using Core.Enums.Lodging;
using Core.Interfaces.Lodging;
using Core.Interfaces.Lodging.UserManagementService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;
using Web.DTOs;

namespace Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserManagementService _userManagementService;
        public UsersController(IMapper mapper, IUserManagementService userManagementService)
        {
            _mapper = mapper;
            _userManagementService = userManagementService;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(ICollection<UserDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        //[AuthorizeRoles(Roles.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userManagementService.GetAllUsers();
            return Ok(_mapper.Map<ICollection<UserDto>>(result.Data));
        }

        [HttpGet("{id}/contacts")]
        [ProducesResponseType(typeof(ICollection<ContactDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetContactsForUser(int id)
        {
            var result = await _userManagementService.GetContacts(id);
            
            if (result.ResultType == Core.Helpers.Results.ResultType.Invalid)
                return BadRequest(new ErrorDto(result.Errors));

            if (result.ResultType == Core.Helpers.Results.ResultType.NotFound)
                return NotFound(new ErrorDto(result.Errors));

            return Ok(_mapper.Map<ICollection<ContactDto>>(result.Data));
        }

        [HttpGet("{id}/approvingdata")]
        [ProducesResponseType(typeof(ICollection<ApprovingDataDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetApprovingDataForUser(int id)
        {
            var result = await _userManagementService.GetApprovingData(id);

            if (result.ResultType == Core.Helpers.Results.ResultType.Invalid)
                return BadRequest(new ErrorDto(result.Errors));

            if (result.ResultType == Core.Helpers.Results.ResultType.NotFound)
                return NotFound(new ErrorDto(result.Errors));

            return Ok(_mapper.Map<ApprovingDataDto>(result.Data));
        }
    }
}
