using System.Collections.Generic;

namespace Core.Entities.LodgingEntities
{
    public class Reservation : BaseEntity
    {
        public string Email { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public ICollection<ReservationItem> ReservationItems { get; set; }
    }
}