using System.Collections.Generic;

namespace Core.Entities.LodgingEntities
{
    public class Room : BaseEntity
    {
        public int AdultCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public float Price { get; set; }
        public int CurrencyId { get; set; }
        public int LodgingId { get; set; }

        public Lodging Lodging { get; set; }
        public Currency Currency { get; set; }
        public ICollection<ReservationItem> ReservationItems { get; set; }
    }
}