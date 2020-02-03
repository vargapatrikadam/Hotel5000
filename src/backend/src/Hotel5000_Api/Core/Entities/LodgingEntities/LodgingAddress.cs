using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class LodgingAddress : BaseEntity
    {
        public string County { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Floor { get; set; }
        public string DoorNumber { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int LodgingId { get; set; }
        public virtual Lodging Lodging { get; set; }
    }
}