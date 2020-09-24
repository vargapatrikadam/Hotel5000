using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
