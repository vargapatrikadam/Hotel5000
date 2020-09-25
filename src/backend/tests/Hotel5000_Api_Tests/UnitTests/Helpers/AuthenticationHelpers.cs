using Auth.Options;

namespace Hotel5000_Api_Tests.UnitTests.Helpers
{
    public static class AuthenticationHelpers
    {
        public static AuthenticationOptions GetTestAuthenticationOptions()
        {
            return new AuthenticationOptions()
            {
                AccessTokenDuration = 60,
                RefreshTokenDuration = 60,
                Secret = "Unit Test Secret"
            };
        }
    }
}
