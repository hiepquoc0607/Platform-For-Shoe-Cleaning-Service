using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Business
{
    public class UpdateBusinessStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
