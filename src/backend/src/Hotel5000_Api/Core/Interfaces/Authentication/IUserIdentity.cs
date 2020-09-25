using Core.Enums.Authentication;

namespace Core.Interfaces.Authentication
{
    public interface IUserIdentity
    {
        public bool IsAuthenticated { get; }
        public RoleType Role { get; }
        public string Username { get; }
        public string Email { get; }
    }
}
