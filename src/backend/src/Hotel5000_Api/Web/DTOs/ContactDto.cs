using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }
        [Required]
        public string MobileNumber { get; set; }
    }
}
