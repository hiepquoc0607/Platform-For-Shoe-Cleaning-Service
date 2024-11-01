using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public class UpdateAccountRequest
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? FullName { get; set; }

        [Phone]
        [StringLength(10, MinimumLength = 10)]
        [DefaultValue("string")]
        public string? Phone { get; set; }

        public DateOnly? Dob { get; set; }

        public string? ImageUrl { get; set; }
    }
}
