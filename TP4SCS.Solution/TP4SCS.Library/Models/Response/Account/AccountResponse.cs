namespace TP4SCS.Library.Models.Response.Account
{
    public class AccountResponse
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsGoogle { get; set; }

        public string? Fcmtoken { get; set; }

        public string Role { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
