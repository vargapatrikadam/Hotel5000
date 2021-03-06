﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class ReservationItemDto
    {
        public int Id { get; set; }
        [Required]
        public DateTime ReservedFrom { get; set; }
        [Required]
        public DateTime ReservedTo { get; set; }
        [Required]
        public int RoomId { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public string LodgingName { get; set; }
        public int ReservationId { get; set; }
    }
}
