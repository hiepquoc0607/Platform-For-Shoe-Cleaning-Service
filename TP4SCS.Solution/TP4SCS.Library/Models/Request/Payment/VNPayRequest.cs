namespace TP4SCS.Library.Models.Request.Payment
{
    public class VnPayRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public int OrderId { get; set; }
    }
}
