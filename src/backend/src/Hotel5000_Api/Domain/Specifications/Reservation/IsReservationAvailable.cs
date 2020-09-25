using Ardalis.Specification;
using Core.Entities.Domain;

namespace Domain.Specifications.Reservation
{
    public class IsReservationAvailable : Specification<ReservationItem>
    {
        public void SetFilter(ReservationItem newReservationItem)
        {
            Query
                .Where(existing => (existing.RoomId == newReservationItem.RoomId) &&
                                   (existing.ReservedFrom < newReservationItem.ReservedTo &&
                                       newReservationItem.ReservedFrom < existing.ReservedTo));
        }
    }
}
