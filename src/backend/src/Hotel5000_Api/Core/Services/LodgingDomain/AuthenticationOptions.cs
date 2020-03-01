﻿namespace Core.Services.LodgingDomain
{
    public class AuthenticationOptions
    {
        public int RefreshTokenDuration { get; set; }
        public int AccessTokenDuration { get; set; }
        public string Secret { get; set; }
    }
}