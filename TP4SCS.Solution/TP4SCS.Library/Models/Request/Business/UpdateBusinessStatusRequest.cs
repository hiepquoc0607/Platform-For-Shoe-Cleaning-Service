using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Business
{
    public class UpdateBusinessStatusRequest
    {
        [Required]
        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
