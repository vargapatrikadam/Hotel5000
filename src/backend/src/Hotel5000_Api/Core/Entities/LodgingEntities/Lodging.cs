using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Lodging : BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int LodgingTypeId { get; set; }

        public User User { get; set; }
        public LodgingType LodgingType { get; set; }
        public virtual ICollection<LodgingAddress> LodgingAddresses { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<ReservationWindow> ReservationWindows { get; set; }
    }
}