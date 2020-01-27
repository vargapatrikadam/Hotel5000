using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
