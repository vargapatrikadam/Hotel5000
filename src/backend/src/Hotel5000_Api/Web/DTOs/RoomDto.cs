using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int LodgingId { get; set; }
        public int AdultCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
    }
}
