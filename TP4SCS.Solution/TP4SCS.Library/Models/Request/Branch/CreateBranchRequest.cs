using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Branch
{
    public class CreateBranchRequest
    {
        [Required]
        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string Ward { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string Province { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string City { get; set; } = string.Empty;

        [Required]
        [DefaultValue(true)]
        public bool IsDeliverySupport { get; set; }
    }
}
