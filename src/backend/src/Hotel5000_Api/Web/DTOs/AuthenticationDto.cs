﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class AuthenticationDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}