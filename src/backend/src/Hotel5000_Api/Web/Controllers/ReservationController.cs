using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        [HttpDelete("{reservatonId}")]
        [ProducesResponseType(typeof(ICollection<ReservationDto>), 200)]
        [ProducesErrorResponseType(typeof(ErrorDto))]
        public async Task<IActionResult> GetReservation(int reservationId)
        {
            var result = await _reservationService.DeleteReservation(reservationId);
            if (result.ResultType != ResultType.Ok)
                return this.GetError(result);

            return Ok();
        }
    }
}