namespace TP4SCS.Library.Models.Request.Service
{
    public class ServiceUpdateRequest
    {
        public int BranchId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string? Status { get; set; }
        public decimal Rating { get; set; }

        public int OrderedNum { get; set; }

        public int FeedbackedNum { get; set; }
        public decimal? NewPrice { get; set; }
        public string? PromotionStatus { get; set; }
    }
}
