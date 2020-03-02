using AutoMapper;
using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces.LodgingDomain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Attributes;
using Web.DTOs;
using Web.Helpers;

namespace Web.Controllers
{
    [Route("api/lodgings")]
    [ApiController]
    public class LodgingsController : ControllerBase
    {
        private readonly ILodgingManagementService _lodgingManagementService;
        private readonly IMapper _mapper;
        public LodgingsController(ILodgingManagementService lodgingManagementService,
            IMapper mapper)
        {
            _lodgingManagementService = lodgingManagementService;
            _mapper = mapper;
        }
        #region get
        [HttpGet("currencies")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetAvailableCurrencies()
        {
            return Ok(_mapper.Map<ICollection<CurrencyDto>>((await _lodgingManagementService.GetCurrency()).Data));
        }
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetLodgings([FromQuery] int? id = null,
            [FromQuery] string name = null,
            [FromQuery] string lodgingType = null,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? resultPerPage = null)
        {
            var result = await _lodgingManagementService.GetLodging(id,
                name,
                lodgingType,
                (pageNumber.HasValue && pageNumber.Value > 0) ? ((pageNumber.Value - 1) * resultPerPage) : null,
                resultPerPage);

            return Ok(_mapper.Map<ICollection<LodgingDto>>(result.Data));
        }
        [HttpGet("{lodgingId}/addresses")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetAddressesForLodging(int lodgingId,
            [FromQuery] int? id = null,
            [FromQuery] string countryCode = null,
            [FromQuery] string countryName = null,
            [FromQuery] string county = null,
            [FromQuery] string city = null,
            [FromQuery] string postalCode = null,
            [FromQuery] string lodgingName = null)
        {
            return Ok(_mapper.Map<ICollection<LodgingAddressDto>>((await _lodgingManagementService.GetLodgingAddress(id, lodgingId, countryCode, countryName, county, city, postalCode, lodgingName)).Data));
        }
        [HttpGet("{lodgingId}/reservationwindows")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetActiveReservationWindowForLodging(int lodgingId)
        {
            return Ok(_mapper.Map<ICollection<ReservationWindowDto>>((await _lodgingManagementService.GetReservationWindow(isAfter: DateTime.Now, lodgingId: lodgingId)).Data));
        }
        [HttpGet("reservationwindows")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetActiveReservationWindows([FromQuery] int? id = null, 
            [FromQuery] int? lodgingId = null, 
            [FromQuery] DateTime? isBefore = null)
        {
            return Ok(_mapper.Map<ICollection<ReservationWindowDto>>((await _lodgingManagementService.GetReservationWindow(id, lodgingId, DateTime.Now, isBefore)).Data));
        }
        [HttpGet("rooms")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetRoom([FromQuery] int? id = null,
            [FromQuery] int? lodgingId = null,
            [FromQuery] int? adultCapacity = null,
            [FromQuery] int? childrenCapacity = null,
            [FromQuery] double? priceMin = null,
            [FromQuery] double? priceMax = null)
        {
            return Ok(_mapper.Map<ICollection<RoomDto>>((await _lodgingManagementService.GetRoom(id, lodgingId, adultCapacity, childrenCapacity, priceMin, priceMax)).Data));
        }
        [HttpGet("{lodgingId}/rooms")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetRoomsForLodging(int lodgingId,
            [FromQuery] int? id = null,
            [FromQuery] int? adultCapacity = null,
            [FromQuery] int? childrenCapacity = null,
            [FromQuery] double? priceMin = null,
            [FromQuery] double? priceMax = null)
        {
            return Ok(_mapper.Map<ICollection<RoomDto>>((await _lodgingManagementService.GetRoom(id, lodgingId, adultCapacity, childrenCapacity, priceMin, priceMax)).Data));
        }
        #endregion
        #region post
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddLodging([FromBody] LodgingDto newLodgingDto)
        {
            var newLodging = _mapper.Map<Lodging>(newLodgingDto);
            var result = await _lodgingManagementService.AddLodging(newLodging, newLodgingDto.LodgingType, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpPost("addresses")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddLodgingAddress([FromBody] LodgingAddressDto newLodgingAddressDto)
        {
            var newLodgingAddress = _mapper.Map<LodgingAddress>(newLodgingAddressDto);
            var result = await _lodgingManagementService.AddLodgingAddress(newLodgingAddress, newLodgingAddressDto.CountryCode, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpPost("reservationwindows")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddReservationWindow([FromBody] ReservationWindowDto newReservationWindowDto)
        {
            var newReservationWindow = _mapper.Map<ReservationWindow>(newReservationWindowDto);
            var result = await _lodgingManagementService.AddReservationWindow(newReservationWindow, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpPost("rooms")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddRoom([FromBody] RoomDto newRoomDto)
        {
            var newRoom = _mapper.Map<Room>(newRoomDto);
            var result = await _lodgingManagementService.AddRoom(newRoom, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        #endregion
        #region delete
        [HttpDelete("rooms/{roomId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var result = await _lodgingManagementService.RemoveRoom(roomId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpDelete("lodgings/{lodgingId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteLodging(int lodgingId)
        {
            var result = await _lodgingManagementService.RemoveLodging(lodgingId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpDelete("lodgingaddresses/{lodgingAddressId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteLodgingAddress(int lodgingAddressId)
        {
            var result = await _lodgingManagementService.RemoveLodgingAddress(lodgingAddressId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpDelete("reservationwindows/{reservationWindowId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> DeleteReservationWindow(int reservationWindowId)
        {
            var result = await _lodgingManagementService.RemoveReservationWindow(reservationWindowId, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        #endregion
        #region put
        [HttpPut("lodgings/{updateThisLodging}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateLodging([FromBody] LodgingDto newLodgingDataDto, int updateThisLodging)
        {
            var newLodgingData = _mapper.Map<Lodging>(newLodgingDataDto);

            var result = await _lodgingManagementService.UpdateLodging(newLodgingData, updateThisLodging, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpPut("rooms/{updateThisRoom}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomDto newRoomDataDto, int updateThisRoom)
        {
            var newRoomData = _mapper.Map<Room>(newRoomDataDto);

            var result = await _lodgingManagementService.UpdateRoom(newRoomData, updateThisRoom, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpPut("reservationwindows/{updateThisReservationWindow}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateRoom([FromBody] ReservationWindowDto newReservationWindowDataDto, int updateThisReservationWindow)
        {
            var newReservationWindowData = _mapper.Map<ReservationWindow>(newReservationWindowDataDto);

            var result = await _lodgingManagementService.UpdateReservationWindow(newReservationWindowData, updateThisReservationWindow, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        [HttpPut("addresses/{updateThisAddress}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateAddress([FromBody] LodgingAddressDto newLodgingAddressDataDto, int updateThisLodgingAddress)
        {
            var newLodgingAddressData = _mapper.Map<LodgingAddress>(newLodgingAddressDataDto);

            var result = await _lodgingManagementService.UpdateLodgingAddress(newLodgingAddressData, updateThisLodgingAddress, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        #endregion
    }
}