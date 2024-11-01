using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Service
{
    public class ServiceUpdateRequest
    {
        public int CategoryId { get; set; }

        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string? Status { get; set; }
        public decimal? NewPrice { get; set; }

        [DefaultValue("string")]
        public string? PromotionStatus { get; set; }
    }
}
