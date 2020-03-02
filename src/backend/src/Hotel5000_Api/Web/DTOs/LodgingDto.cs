using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class LodgingDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LodgingType { get; set; }
        [Required]
        public int UserId{ get; set; }
        public ICollection<RoomDto> Rooms { get; set; }
        public ICollection<LodgingAddressDto> LodgingAddresses { get; set; }
        public ICollection<ReservationWindowDto> ReservationWindows { get; set; }
    }
}
