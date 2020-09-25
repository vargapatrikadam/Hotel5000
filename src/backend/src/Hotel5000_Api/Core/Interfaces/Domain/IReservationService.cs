﻿using Core.Entities.Domain;
using Core.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Domain
{
    public interface IReservationService
    {
        Task<Result<bool>> Reserve(Reservation newReservation, string paymentType);
        Task<Result<bool>> DeleteReservation(int reservationId);
        Task<Result<IReadOnlyList<Reservation>>> GetReservation(int? id = null,
            int? roomId = null,
            int? reservationWindowId = null,
            string email = null,
            int? lodgingId = null);
        Task<Result<IReadOnlyList<ReservationWindow>>> GetAvailableReservationWindowsForRoom(int roomId);

    }
}
