using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Reservation : BaseEntity
    {
        public DateTime ReservedFrom { get; set; }
        public DateTime ReservedTo { get; set; }

        public int ReservationWindowId { get; set; }
        public virtual ReservationWindow ReservationWindow { get; set; }
        public int UserReservationId { get; set; }
        public virtual UserReservation UserReservation { get; set; }
    }
}