using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [DefaultValue("string")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [DefaultValue("string")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
