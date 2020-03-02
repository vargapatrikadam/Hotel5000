using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        [Required]
        public int LodgingId { get; set; }
        [Required]
        public int AdultCapacity { get; set; }
        [Required]
        public int ChildrenCapacity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Currency { get; set; }
    }
}
