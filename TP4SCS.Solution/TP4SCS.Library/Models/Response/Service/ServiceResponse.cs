using TP4SCS.Library.Models.Response.AssetUrl;
using TP4SCS.Library.Models.Response.Promotion;

namespace TP4SCS.Library.Models.Response.Service
{
    public class ServiceResponse
    {
        public int Id { get; set; }

        public required int BranchId { get; set; }
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal Rating { get; set; }

        public DateTime CreateTime { get; set; }

        public int OrderedNum { get; set; }

        public int FeedbackedNum { get; set; }

        public string Status { get; set; } = null!;

        public PromotionResponse? Promotion { get; set; }

        public List<AssetUrlResponse>? AssetUrl { get; set; }
    }
}
