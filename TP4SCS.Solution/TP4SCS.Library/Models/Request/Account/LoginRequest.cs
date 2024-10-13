using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
