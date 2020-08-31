using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Models.Domain
{
    public class Lodging
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int LodgingTypeId { get; set; }

        public ICollection<LodgingAddress> LodgingAddresses { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<ReservationWindow> ReservationWindows { get; set; }
    }
}
