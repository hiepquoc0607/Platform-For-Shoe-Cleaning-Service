namespace TP4SCS.Library.Models.Request.Promotion
{
    public class PromotionCreateRequest
    {
        public int ServiceId { get; set; }

        public int SaleOff { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
