using System;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class ReservationWindowDto
    {
        public int Id { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        public int LodgingId { get; set; }

    }
}
