namespace TP4SCS.Library.Models.Request.Promotion
{
    public class PromotionUpdateRequest
    {
        public int SaleOff { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; } = null!;
    }
}
