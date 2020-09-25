using Ardalis.Specification;
using System.Linq;

namespace Domain.Specifications.Reservation
{
    public class GetReservation : Specification<Core.Entities.Domain.Reservation>
    {
        public GetReservation(int? id = null,
            int? roomId = null,
            int? reservationWindowId = null,
            string email = null,
            int? lodgingId = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id) &&
                        (!roomId.HasValue || p.ReservationItems.Any(o => o.RoomId == roomId && o.ReservationId == p.Id)) &&
                        (!reservationWindowId.HasValue || p.ReservationItems.Any(o => o.ReservationWindowId == reservationWindowId && o.ReservationId == p.Id)) &&
                        (email == null || p.Email == email) &&
                        (!lodgingId.HasValue || p.ReservationItems.Any(o => o.Room.LodgingId == lodgingId && o.ReservationId == p.Id)));
            Query
                .Include(i => i.ReservationItems)
                    .ThenInclude(i => i.Room)
                        .ThenInclude(i => i.Lodging);
            Query
                .Include(i => i.ReservationItems)
                    .ThenInclude(i => i.Room)
                        .ThenInclude(i => i.Currency);
            Query
                .Include(i => i.ReservationItems)
                    .ThenInclude(i => i.ReservationWindow);
        }
    }
}
