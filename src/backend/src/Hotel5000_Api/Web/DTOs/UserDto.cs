using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string IdentityNumber { get; set; }
        public string TaxNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public ICollection<ContactDto> Contacts { get; set; }
        //public virtual ICollection<LodgingDto> Lodgings { get; set; }
    }
}
