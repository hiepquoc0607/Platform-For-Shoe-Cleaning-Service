using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Branch
{
    public class UpdateBranchStatusRequest
    {
        [Required]
        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
