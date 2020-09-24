using Core.Enums.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Authentication
{
    public interface IUserIdentity
    {
        public bool IsAuthenticated { get; }
        public RoleType Role{ get; }
        public string Username { get; }
        public string Email { get; }
    }
}
