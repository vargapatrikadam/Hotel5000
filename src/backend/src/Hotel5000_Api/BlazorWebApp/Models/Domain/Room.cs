using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Models.Domain
{
    public class Room
    {
        public int AdultCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public float Price { get; set; }
        public int CurrencyId { get; set; }
        public int LodgingId { get; set; }
    }
}
