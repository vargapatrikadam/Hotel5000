using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class LodgingAddressDto
    {
        public int Id { get; set; }
        [Required]
        public int LodgingId { get; set; }
        public string Country { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        public string Floor { get; set; }
        public string DoorNumber { get; set; }
    }
}
