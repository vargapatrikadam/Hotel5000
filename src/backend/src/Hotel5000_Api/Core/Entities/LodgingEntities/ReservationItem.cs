using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class ReservationItem : BaseEntity
    {
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public int ReservationWindowId { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Room Room { get; set; }
        public virtual ReservationWindow ReservationWindow { get; set; }
    }
}