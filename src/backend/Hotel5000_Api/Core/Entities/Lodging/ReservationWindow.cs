using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Lodging
{
    public class ReservationWindow : BaseEntity
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Price { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
