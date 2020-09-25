using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public ICollection<ReservationItemDto> ReservationItems { get; set; }
    }
}
