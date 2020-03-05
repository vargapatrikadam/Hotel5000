using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain
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
    }
}
