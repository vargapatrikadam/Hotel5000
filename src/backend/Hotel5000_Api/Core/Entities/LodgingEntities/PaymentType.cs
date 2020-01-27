using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class PaymentType : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<UserReservation> UserReservations { get; set; }
    }
}
