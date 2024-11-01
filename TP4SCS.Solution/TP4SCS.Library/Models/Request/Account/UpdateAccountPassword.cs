using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public class UpdateAccountPassword
    {
        [Required]
        [DefaultValue("string")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
