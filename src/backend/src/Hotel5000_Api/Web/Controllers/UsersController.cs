using AutoMapper;
using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using Core.Helpers.Results;
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
        public async Task<IActionResult> GetUsers([FromQuery] int? id = null, 
            [FromQuery] string username = null, 
            [FromQuery] string email = null,
            [FromQuery] int? pageNumber = null, 
            [FromQuery] int? resultPerPage = null)
        {
            var result = await _userManagementService.GetUsers(id, 
                username, 
                email, 
                (pageNumber.HasValue && pageNumber.Value > 0) ? ((pageNumber.Value - 1) * resultPerPage) : null,
                resultPerPage);

            return Ok(_mapper.Map<ICollection<UserDto>>(result.Data));
        }
        
        [HttpGet("{id}/contacts")]
        [ProducesResponseType(typeof(ICollection<ContactDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetContactsForUser(int id)
        {
            var result = await _userManagementService.GetContacts(userId: id);

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok(_mapper.Map<ICollection<ContactDto>>(result.Data));
        }
        
        [HttpGet("contacts")]
        [ProducesResponseType(typeof(ICollection<ContactDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetContacts([FromQuery] int? userId = null, 
            [FromQuery] string phoneNumber = null, 
            [FromQuery] string username = null)
        {
            var result = await _userManagementService.GetContacts(userId, phoneNumber, username);

            return Ok(_mapper.Map<ICollection<ContactDto>>(result.Data));
        }
        
        [HttpGet("{userId}/approvingdata")]
        [ProducesResponseType(typeof(ICollection<ApprovingDataDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetApprovingDataForUser(int userId)
        {
            var result = await _userManagementService.GetApprovingData(approvingDataOwnerId: userId);

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok(_mapper.Map<ICollection<ApprovingDataDto>>(result.Data));
        }

        [HttpGet("approvingdata")]
        [ProducesResponseType(typeof(ICollection<ApprovingDataDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetApprovingData([FromQuery] int? userId = null,
            [FromQuery] int? id = null,
            [FromQuery] string username = null,
            [FromQuery] string taxNumber = null,
            [FromQuery] string identityNumber = null,
            [FromQuery] string registrationNumber = null)
        {
            var result = await _userManagementService.GetApprovingData(approvingDataOwnerId: userId,
               approvingDataId: id,
               username: username,
               taxNumber: taxNumber,
               identityNumber: identityNumber,
               registrationNumber: registrationNumber);

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok(_mapper.Map<ICollection<ApprovingDataDto>>(result.Data));
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userManagementService.RemoveUser(userId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok();
        }

        [HttpDelete("{userId}/approvingdata")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteApprovingData(int userId)
        {
            var result = await _userManagementService.RemoveApprovingData(userId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok();
        }

        [HttpDelete("{userId}/contacts/{contactId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteContact(int userId, int contactId)
        {
            var result = await _userManagementService.RemoveContact(userId, contactId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok();
        }

        [HttpPut("contacts/{contactId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateContact([FromBody] ContactDto updatedContact, int contactId)
        {
            var result = await _userManagementService.UpdateContact(_mapper.Map<Contact>(updatedContact), contactId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok();
        }
        [HttpPut("approvingdata/{approvingDataId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateApprovingData([FromBody] ApprovingDataDto updatedApprovingData, int approvingDataId)
        {
            var result = await _userManagementService.UpdateApprovingData(_mapper.Map<ApprovingData>(updatedApprovingData), approvingDataId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok();
        }
        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto updatedUser, int userId)
        {
            var result = await _userManagementService.UpdateUser(_mapper.Map<User>(updatedUser), userId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return GetError(result);

            return Ok();
        }

        private IActionResult GetError<T>(Result<T> result)
        {
            switch (result.ResultType)
            {
                case ResultType.Invalid: return BadRequest(new ErrorDto(result.Errors)); 
                case ResultType.NotFound: return NotFound(new ErrorDto(result.Errors));
                case ResultType.Unauthorized: return Unauthorized(new ErrorDto(result.Errors));
                case ResultType.Unexpected: return BadRequest(new ErrorDto(result.Errors));
                case ResultType.Conflict: return Conflict(new ErrorDto(result.Errors));
                default: return BadRequest(new ErrorDto(result.Errors));
            }
        }
    }
}
