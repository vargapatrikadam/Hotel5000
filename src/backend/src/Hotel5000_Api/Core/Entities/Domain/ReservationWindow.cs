using System;
using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class ReservationWindow : BaseEntity
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int LodgingId { get; set; }

        public Lodging Lodging { get; set; }
        public ICollection<ReservationItem> ReservationItems { get; set; }
    }
}