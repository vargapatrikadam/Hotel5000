using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces.LodgingDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs;
using Web.Helpers;

namespace Web.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService,
            IMapper mapper)
        {
            _mapper = mapper;
            _reservationService = reservationService;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(ICollection<ReservationDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetReservation([FromQuery] int? id = null,
            [FromQuery] int? roomId = null,
            [FromQuery] int? reservationWindowId = null,
            [FromQuery] string email = null,
            [FromQuery] int? lodgingId = null)
        {
            var result = await _reservationService.GetReservation(id,
                roomId,
                reservationWindowId,
                email,
                lodgingId);

            return Ok(_mapper.Map<ICollection<ReservationDto>>(result.Data));
        }
        [HttpDelete("{reservationId}")]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            var result = await _reservationService.DeleteReservation(reservationId);
            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
        /// <summary>
        /// Adds a new reservation
        /// </summary>
        /// <param name="newReservation">contains the data for a new reservation</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "email": "new@reservation.com",
        ///         "paymentType": "Cash",
        ///         "reservationItems": [
        ///             {
        ///                 "reservedFrom": "2020-03-06",
        ///                 "reservedTo": "2020-03-10",
        ///                 "roomId": 10
        ///             },
        ///             {
        ///                 "reservedFrom": "2020-03-11",
        ///                 "reservedTo": "2020-03-20",
        ///                 "roomId": 10
        ///             }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetReservation([FromBody] ReservationDto newReservation)
        {
            var result = await _reservationService.Reserve(_mapper.Map<Reservation>(newReservation), newReservation.PaymentType);
            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
    }
}