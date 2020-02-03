using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class PaymentType : BaseEntity
    {
        public PaymentTypes Name { get; set; }

        public virtual ICollection<UserReservation> UserReservations { get; set; }
    }
}