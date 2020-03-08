namespace Web.DTOs
{
    public class AuthenticationDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}