using Ardalis.Specification;
using Core.Entities.Domain;

namespace Domain.Specifications.Reservation
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
