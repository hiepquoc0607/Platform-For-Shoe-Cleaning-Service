namespace TP4SCS.Library.Models.Response.Auth
{
    public class AuthResponse
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public string Token { get; set; } = string.Empty;

    }
}
