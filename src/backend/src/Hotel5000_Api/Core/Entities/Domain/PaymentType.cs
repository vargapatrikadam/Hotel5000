using Core.Enums.Lodging;
using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class PaymentType : BaseEntity
    {
        public PaymentTypes Name { get; set; }

        public ICollection<Reservation> UserReservations { get; set; }
    }
}