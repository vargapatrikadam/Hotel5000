using Core.Enums.Lodging;
using System.Collections.Generic;

namespace Core.Entities.LodgingEntities
{
    public class Role : BaseEntity
    {
        public Roles Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}