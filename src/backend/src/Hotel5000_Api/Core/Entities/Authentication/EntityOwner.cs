using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Authentication
{
    public class EntityOwner
    {
        public EntityOwner()
        {
        }
        public EntityOwner(string username)
        {
            Username = username;
        }
        public EntityOwner(int userId)
        {
            UserId = userId;
        }
        public EntityOwner(string username, int userId)
        {
            UserId = userId;
            Username = username;
        }
        public string Username { get; set; }
        public int UserId { get; set; }
    }
}
