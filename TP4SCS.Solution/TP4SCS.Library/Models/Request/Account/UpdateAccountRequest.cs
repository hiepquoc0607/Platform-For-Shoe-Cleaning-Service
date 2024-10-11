namespace TP4SCS.Library.Models.Request.Account
{
    public class UpdateAccountRequest
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public DateOnly? Dob { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public string? ImageUrl { get; set; }

        public bool? IsGoogle { get; set; }

        public string? Fcmtoken { get; set; }
    }
}
