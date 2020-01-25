using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Lodging
{
    public class Role
    {
        public RoleId Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
