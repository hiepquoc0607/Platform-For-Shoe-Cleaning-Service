namespace TP4SCS.Library.Models.Request.Branch
{
    public class UpdateBranchRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Ward { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public bool IsDeliverySupport { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
