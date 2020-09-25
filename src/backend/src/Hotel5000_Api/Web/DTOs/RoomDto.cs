using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int LodgingId { get; set; }
        [Required]
        public int AdultCapacity { get; set; }
        [Required]
        public int ChildrenCapacity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Currency { get; set; }
        public ICollection<ReservationWindowDto> ReservationWindows { get; set; }
    }
}
