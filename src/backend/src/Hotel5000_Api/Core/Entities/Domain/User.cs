﻿using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class User : BaseEntity
    {
        public User()
        {

        }
        public User(int id)
        {
            this.Id = id;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ApprovingData ApprovingData { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Lodging> Lodgings { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }
}