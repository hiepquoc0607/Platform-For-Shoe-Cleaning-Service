using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Service
{
    public class ServiceCreateRequest
    {
        public int[] BranchId { get; set; } = Array.Empty<int>();

        public int CategoryId { get; set; }

        [DefaultValue("string")]
        public string Name { get; set; } = null!;

        [DefaultValue("string")]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;

        public decimal? NewPrice { get; set; }
    }
}
