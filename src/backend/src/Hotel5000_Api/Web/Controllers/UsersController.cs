using AutoMapper;
using Core.Entities.LodgingEntities;
using Core.Helpers;
using Core.Helpers.Results;
using Core.Interfaces.LodgingDomain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;
using Web.DTOs;
using Web.Helpers;

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

            return Ok(_mapper.Map<ICollection<UserDto>>(result.Data.Select(p => p.WithoutPassword())));
        }

        [HttpGet("{id}/contacts")]
        [ProducesResponseType(typeof(ICollection<ContactDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetContactsForUser(int id)
        {
            var result = await _userManagementService.GetContacts(userId: id);

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok(_mapper.Map<ICollection<ContactDto>>(result.Data));
        }

        [HttpGet("contacts")]
        [ProducesResponseType(typeof(ICollection<ContactDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetContacts([FromQuery] int? id = null,
            [FromQuery] int? userId = null,
            [FromQuery] string phoneNumber = null,
            [FromQuery] string username = null)
        {
            var result = await _userManagementService.GetContacts(id, userId, phoneNumber, username);

            return Ok(_mapper.Map<ICollection<ContactDto>>(result.Data));
        }

        [HttpGet("{userId}/approvingdata")]
        [ProducesResponseType(typeof(ICollection<ApprovingDataDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetApprovingDataForUser(int userId)
        {
            var result = await _userManagementService.GetApprovingData(approvingDataOwnerId: userId);

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

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
                return this.GetError(result);

            return Ok(_mapper.Map<ICollection<ApprovingDataDto>>(result.Data));
        }
        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="userId">the id of the user who we want to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/users/2
        ///
        /// </remarks>
        [HttpDelete("{userId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userManagementService.RemoveUser(userId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        /// <summary>
        /// Deletes the approving data for an user
        /// </summary>
        /// <param name="userId">the id of the user from which we'll delete the approving data</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/users/2/approvingdata
        ///
        /// </remarks>
        [HttpDelete("{userId}/approvingdata")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteApprovingData(int userId)
        {
            var result = await _userManagementService.RemoveApprovingData(userId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        /// <summary>
        /// Delete a contatct from an user
        /// </summary>
        /// <param name="userId">the id of the user who has the contact</param>
        /// <param name="contactId">the id of the contact</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/users/2/contacts/3
        ///
        /// </remarks>
        [HttpDelete("{userId}/contacts/{contactId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteContact(int userId, int contactId)
        {
            var result = await _userManagementService.RemoveContact(userId, contactId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }

        /// <summary>
        /// Updates a contact 
        /// </summary>
        /// <param name="updatedContact">this contains the new data for contact</param>
        /// <param name="contactId">this is the unique id of the contact we modify</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/users/contacts/2
        ///     {
        ///        "mobileNumber": "06 30 555 555" //unique
        ///     }
        ///
        /// </remarks>
        [HttpPut("contacts/{contactId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateContact([FromBody] ContactDto updatedContact, int contactId)
        {
            var result = await _userManagementService.UpdateContact(_mapper.Map<Contact>(updatedContact), contactId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }

        /// <summary>
        /// Updates the only approving data for an user
        /// </summary>
        /// <param name="updatedApprovingData">this contains the new approving data</param>
        /// <param name="approvingDataId">this is the unique id of the approving data we modify</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/users/approvingdata/2
        ///     {
        ///        "identityNumber": "12345678", //unique, 8 digits, nullable
        ///        "taxNumber" : "1234567891234", //unique, 13 digits, nullable
        ///        "registrationNumber" : "123456789123", //unique, 12 digits, nullable
        ///     }
        ///
        /// </remarks>
        [HttpPut("approvingdata/{approvingDataId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateApprovingData([FromBody] ApprovingDataDto updatedApprovingData, int approvingDataId)
        {
            var result = await _userManagementService.UpdateApprovingData(_mapper.Map<ApprovingData>(updatedApprovingData), approvingDataId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }

        /// <summary>
        /// Updates an user
        /// </summary>
        /// <param name="updatedUser">this contains the data for the new user data</param>
        /// <param name="userId">this is the unique id of the user we modify</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/users/2
        ///     {
        ///        "email": "test@email.com", //unique
        ///        "firstName" : "new firstname", 
        ///        "lastName" : "new lastname",
        ///        "username" : "new username" //unique
        ///     }
        ///
        /// </remarks>
        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto updatedUser, int userId)
        {
            var result = await _userManagementService.UpdateUser(_mapper.Map<User>(updatedUser), userId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }

        /// <summary>
        /// Adds a new contact for an user
        /// </summary>
        /// <param name="newContactDto">this contains the data for a new contact</param>
        /// <param name="userId">unique id of the user we wan to add the contact to</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/users/2/contacts
        ///     {
        ///        "mobileNumber": "06 30 555 555"
        ///     }
        ///
        /// </remarks>
        [HttpPost("{userId}/contacts")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddContact([FromBody] ContactDto newContactDto, int userId)
        {
            var newContact = _mapper.Map<Contact>(newContactDto);
            newContact.UserId = userId;
            var result = await _userManagementService.AddContact(newContact, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }

        /// <summary>
        /// Adds a new approving data for an user, only one can exists at a time for any user
        /// </summary>
        /// <param name="newApprovingDataDto">this contains the data for a new approving data</param>
        /// <param name="userId">unique id of the user we'll add the approving data to</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/users/2/approvingdata
        ///     {
        ///        "identityNumber": "12345678", //unique, 8 digits, nullable
        ///        "taxNumber" : "1234567891234", //unique, 13 digits, nullable
        ///        "registrationNumber" : "123456789123", //unique, 12 digits, nullable
        ///     }
        ///
        /// </remarks>
        [HttpPost("{userId}/approvingdata")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddApprovingData([FromBody] ApprovingDataDto newApprovingDataDto, int userId)
        {
            var newApprovingData = _mapper.Map<ApprovingData>(newApprovingDataDto);
            newApprovingData.UserId = userId;
            var result = await _userManagementService.AddApprovingData(newApprovingData, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
    }
}
