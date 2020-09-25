﻿using System.Collections.Generic;

namespace BlazorWebApp.Models
{
    public class UserInfo
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public IEnumerable<ClaimValue> Claims { get; set; }

    }
}
