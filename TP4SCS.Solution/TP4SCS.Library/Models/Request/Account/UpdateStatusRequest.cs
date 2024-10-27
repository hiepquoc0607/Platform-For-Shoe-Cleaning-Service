using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public class UpdateStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
