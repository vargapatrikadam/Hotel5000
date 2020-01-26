using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ApprovingData ApprovingData { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Lodging> Lodgings { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
