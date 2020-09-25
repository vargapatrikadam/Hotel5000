using Core.Enums.Authentication;
using Core.Interfaces.Authentication;
using System;

namespace Auth.Identity
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity(string role, string username, string email) :
            this(Enum.IsDefined(typeof(RoleType), role) ? (RoleType)Enum.Parse(typeof(RoleType), role) : RoleType.ANONYMOUS,
                username,
                email)
        { }
        public UserIdentity(RoleType role, string username, string email)
        {
            IsAuthenticated = true;
            if (role == RoleType.ANONYMOUS)
                IsAuthenticated = false;
            Role = role;
            Username = username;
            Email = email;
        }
        public bool IsAuthenticated { get; }

        public RoleType Role { get; }

        public string Username { get; }

        public string Email { get; }
    }
}
