using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Role : BaseEntity
    {
        public Roles Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}