using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Reservation
{
    public class GetReservationItem : Specification<ReservationItem>
    {
        public GetReservationItem(int reservationWindowId, int roomId)
        {
            Query
                .Where(p => p.ReservationWindowId == reservationWindowId &&
                            p.RoomId == roomId);
        }
    }
}
