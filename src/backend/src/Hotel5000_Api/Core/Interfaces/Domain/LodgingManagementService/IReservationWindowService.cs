using Core.Entities.Domain;
using Core.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Domain.LodgingManagementService
{
    public interface IReservationWindowService
    {
        Task<Result<IReadOnlyList<ReservationWindow>>> GetReservationWindow(int? id = null,
            int? lodgingId = null,
            DateTime? isAfter = null,
            DateTime? isBefore = null);
        Task<Result<bool>> UpdateReservationWindow(ReservationWindow newReservationWindow,
        int oldReservationWindowId);

        Task<Result<bool>> RemoveReservationWindow(int reservationWindowId);

        Task<Result<bool>> AddReservationWindow(ReservationWindow reservationWindow);
    }
}
