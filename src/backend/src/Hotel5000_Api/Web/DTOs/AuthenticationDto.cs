﻿namespace Web.DTOs
{
    public class AuthenticationDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}