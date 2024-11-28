using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Auth
{
    public class LoginOTPRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(100000, 999999)]
        [DefaultValue(0)]
        public int OTP { get; set; }
    }
}
