﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
