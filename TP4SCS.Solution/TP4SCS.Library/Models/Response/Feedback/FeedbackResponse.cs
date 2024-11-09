using TP4SCS.Library.Models.Response.AssetUrl;

namespace TP4SCS.Library.Models.Response.Feedback
{
    public class FeedbackResponse
    {
        public int Id { get; set; }

        public int OrderItemId { get; set; }

        public decimal Rating { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool IsValidContent { get; set; }

        public bool IsValidAsset { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<AssetUrlResponse> AssetUrls { get; set; } = new List<AssetUrlResponse>();
    }
}
