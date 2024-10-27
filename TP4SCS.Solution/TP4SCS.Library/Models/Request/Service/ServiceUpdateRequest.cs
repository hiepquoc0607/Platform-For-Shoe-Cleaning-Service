using TP4SCS.Library.Models.Request.Promotion;

namespace TP4SCS.Library.Models.Request.Service
{
    public class ServiceUpdateRequest
    {

        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }

        public string? Description { get; set; }
        public string? Status { get; set; }

        public decimal Price { get; set; }

        public decimal Rating { get; set; }

        public int OrderedNum { get; set; }

        public int FeedbackedNum { get; set; }
        public PromotionUpdateRequest? PromotionUpdateRequest { get; set; }
    }
}
