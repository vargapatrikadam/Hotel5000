using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class ReservationWindow : BaseEntity
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int LodgingId { get; set; }

        public virtual Lodging Lodging { get; set; }
        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
    }
}