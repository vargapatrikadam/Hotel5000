using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class UserReservation : BaseEntity
    {
        public string Email { get; set; }

        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentType{ get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
