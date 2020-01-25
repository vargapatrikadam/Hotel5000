using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Lodging
{
    public class PaymentType
    {
        public PaymentTypeId Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserReservation> UserReservations { get; set; }
    }
}
