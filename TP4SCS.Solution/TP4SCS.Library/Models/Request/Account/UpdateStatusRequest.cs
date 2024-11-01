using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Account
{
    public class UpdateStatusRequest
    {
        [Required]
        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
