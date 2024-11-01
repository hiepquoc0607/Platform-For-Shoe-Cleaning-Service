using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Branch
{
    public class UpdateBranchRequest
    {
        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string Address { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string Ward { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string Province { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string City { get; set; } = string.Empty;

        [DefaultValue(true)]
        public bool IsDeliverySupport { get; set; }

        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
