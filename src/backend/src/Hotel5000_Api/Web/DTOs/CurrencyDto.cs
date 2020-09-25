using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
