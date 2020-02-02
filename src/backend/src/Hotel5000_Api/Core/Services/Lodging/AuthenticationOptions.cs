using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Lodging
{
    public class AuthenticationOptions
    {
        public int RefreshTokenDuration { get; set; }
        public int AccessTokenDuration { get; set; }
        public string Secret { get; set; }
    }
}