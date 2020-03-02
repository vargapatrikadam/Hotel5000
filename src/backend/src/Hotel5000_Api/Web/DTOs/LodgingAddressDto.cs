using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class LodgingAddressDto
    {
        public int Id { get; set; }
        public int LodgingId { get; set; }
        public string CountryCode { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Floor { get; set; }
        public string DoorNumber { get; set; }
    }
}
