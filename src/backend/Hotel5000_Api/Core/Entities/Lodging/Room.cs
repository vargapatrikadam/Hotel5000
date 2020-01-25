using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Lodging
{
    public class Room : BaseEntity
    {
        public int AdultCapacity { get; set; }
        public int ChildrenCapacity { get; set; }

        public int LodgingId { get; set; }
        public virtual Lodging Lodging { get; set; }
        public virtual ICollection<ReservationWindow> ReservationWindows { get; set; }
    }
}
