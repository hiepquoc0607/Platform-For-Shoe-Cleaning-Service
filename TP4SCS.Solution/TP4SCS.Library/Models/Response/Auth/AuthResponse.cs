namespace TP4SCS.Library.Models.Response.Auth
{
    public class AuthResponse
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateOnly Dob { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }

        public string? Fcmtoken { get; set; }

        public string Role { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public string Token { get; set; } = string.Empty;

    }
}
