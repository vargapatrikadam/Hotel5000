using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
