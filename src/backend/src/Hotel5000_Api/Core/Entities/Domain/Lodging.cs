using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class Lodging : BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int LodgingTypeId { get; set; }

        public User User { get; set; }
        public LodgingType LodgingType { get; set; }
        public ICollection<LodgingAddress> LodgingAddresses { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<ReservationWindow> ReservationWindows { get; set; }
    }
}