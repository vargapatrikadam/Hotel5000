using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class RefreshDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
