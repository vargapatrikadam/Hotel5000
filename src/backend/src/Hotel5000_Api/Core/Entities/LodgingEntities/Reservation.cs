using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Reservation : BaseEntity
    {
        public string Email { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
    }
}