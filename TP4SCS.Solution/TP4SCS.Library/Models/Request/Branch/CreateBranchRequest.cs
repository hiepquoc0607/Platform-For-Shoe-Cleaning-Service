using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Branch
{
    public class CreateBranchRequest
    {
        private string _name = string.Empty;
        [Required]
        [DefaultValue("BranchName")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [Required]
        [DefaultValue("Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DefaultValue("Ward")]
        public string Ward { get; set; } = string.Empty;

        [Required]
        [DefaultValue("Province")]
        public string Province { get; set; } = string.Empty;

        [Required]
        [DefaultValue("City")]
        public string City { get; set; } = string.Empty;

        [Required]
        [DefaultValue(true)]
        public bool IsDeliverySupport { get; set; }
    }
}
