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
        /// <summary>
        /// Add a new lodging
        /// </summary>
        /// <param name="newLodgingDto">this contains the data for a new lodging</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/lodgings
        ///     {
        ///        "name": "name of the new lodging",
        ///        "lodgingType" : "type of the new lodging", //Private or Company 
        ///        "rooms" : [                        // optional
        ///             {
        ///                 "adultCapacity" : 2,
        ///                 "childrenCapacity" : 0,
        ///                 "price" : 200,
        ///                 "currency": "Forint"
        ///             },
        ///             {
        ///                 "adultCapacity" : 0,
        ///                 "childrenCapacity" : 2,
        ///                 "price" : 1,
        ///                 "currency": "Euro"
        ///             }
        ///        ],
        ///        "lodgingAddresses": [             // optional
        ///             {
        ///                 "countryCode": "HU",
        ///                 "county": "Nógrád",
        ///                 "city": "Balassagyarmat",
        ///                 "postalCode" : 2660,
        ///                 "street": "Bajcsy-Zsilinszki utca",
        ///                 "houseNumber": "1"
        ///             },
        ///             {
        ///                 "countryCode": "HU",
        ///                 "county": "Nógrád",
        ///                 "city": "Balassagyarmat",
        ///                 "postalCode" : 2660,
        ///                 "street": "Bajcsy-Zsilinszki utca",
        ///                 "houseNumber": "2",
        ///                 "floor": "3",
        ///                 "doorNumber": "4"
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddLodging([FromBody] LodgingDto newLodgingDto)
        {
            newLodgingDto.UserId = int.Parse(User.Identity.Name);
            var newLodging = _mapper.Map<Lodging>(newLodgingDto);
            var result = await _lodgingManagementService.AddLodging(newLodging, newLodgingDto.LodgingType, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        /// <summary>
        /// Adds a new lodging address to a lodging
        /// </summary>
        /// <param name="newLodgingAddressDto">This contains the data for a new lodging address</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/lodgings/addresses
        ///     {
        ///         "lodgingId" : 1
        ///         "countryCode": "HU",
        ///         "county": "Nógrád",
        ///         "city": "Balassagyarmat",
        ///         "postalCode" : 2660,
        ///         "street": "Bajcsy-Zsilinszki utca",
        ///         "houseNumber": "1"
        ///      }
        ///      
        /// </remarks>
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
        /// <summary>
        /// Adds a new reservation window to a lodging
        /// </summary>
        /// <param name="newReservationWindowDto">This contains the data for a new reservation window</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/lodgings/reservationwindows
        ///     {
        ///         "lodgingId" : 1
        ///         "From": "2020-9-1"
        ///         "To": "2020-10-1"
        ///      }
        ///      
        /// </remarks>
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
        /// <summary>
        /// Adds a new room to a lodging
        /// </summary>
        /// <param name="newRoomDto">This contains the data for a new room</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/lodgings/rooms
        ///     {
        ///         "lodgingId" : 1
        ///         "adultCapacity" : 0,
        ///         "childrenCapacity" : 2,
        ///         "price" : 1,
        ///         "currency": "Euro"
        ///      }
        ///      
        /// </remarks>
        [HttpPost("rooms")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> AddRoom([FromBody] RoomDto newRoomDto)
        {
            var newRoom = _mapper.Map<Room>(newRoomDto);
            var result = await _lodgingManagementService.AddRoom(newRoom, newRoomDto.Currency,int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        #endregion
        #region delete
        /// <summary>
        /// Deletes a room from a lodging
        /// </summary>
        /// <param name="roomId">id of the room we want to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     DELETE api/lodgings/rooms/2
        ///     
        /// </remarks>
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
        /// <summary>
        /// Deletes a lodging
        /// </summary>
        /// <param name="lodgingId">id of the lodging we want to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     DELETE api/lodgings/2
        ///     
        /// </remarks>
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
        /// <summary>
        /// Updates a lodging
        /// </summary>
        /// <param name="newLodgingDataDto">contains the data for an updated lodging</param>
        /// <param name="updateThisLodging">id of the lodging we update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/lodgings/2
        ///     {
        ///         "name": "updated lodging name"
        ///     }
        ///     
        /// </remarks>
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
        /// <summary>
        /// Updates a room for a lodging
        /// </summary>
        /// <param name="newRoomDataDto">contains the data for an updated room</param>
        /// <param name="updateThisRoom">id of the room we update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/lodgings/rooms/2
        ///     {
        ///         "adultCapacity" : 3,
        ///         "childrenCapacity" : 2,
        ///         "price" : 10,
        ///         "currency": "Euro"
        ///     }
        ///     
        /// </remarks>
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
        /// <summary>
        /// Updates a reservation window for a lodging
        /// </summary>
        /// <param name="newReservationWindowDataDto">contains the data for an updated reservation window</param>
        /// <param name="updateThisReservationWindow">id of the reservation window we update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/lodgings/reservationwindows/2
        ///     {
        ///         "from": "2020-4-1",
        ///         "to": "2020-5-1"
        ///     }
        /// 
        /// </remarks>
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
        /// <summary>
        /// Updates an address for a lodging
        /// </summary>
        /// <param name="newLodgingAddressDataDto">This contains the data for an updated lodging address</param>
        /// <param name="updateThisAddress">id of the lodging address we want to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/lodgings/addresses/18
        ///     {
        ///         "countryCode": "HU",
        ///         "county": "Nógrád",
        ///         "city": "Balassagyarmat",
        ///         "postalCode": "2660",
        ///         "street": "Bajcsy-Zsilinszki utca",
        ///         "houseNumber": "2",
        ///         "floor": "3", //optional
        ///         "doorNumber": "22" //optional
        ///     }
        ///
        /// </remarks>
        [HttpPut("addresses/{updateThisAddress}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        [AuthorizeRoles]
        public async Task<IActionResult> UpdateAddress([FromBody] LodgingAddressDto newLodgingAddressDataDto, int updateThisAddress)
        {
            var newLodgingAddressData = _mapper.Map<LodgingAddress>(newLodgingAddressDataDto);

            var result = await _lodgingManagementService.UpdateLodgingAddress(newLodgingAddressData, updateThisAddress, int.Parse(User.Identity.Name));

            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        #endregion
    }
}
