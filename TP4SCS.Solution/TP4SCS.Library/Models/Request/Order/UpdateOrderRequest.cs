namespace TP4SCS.Library.Models.Request.Order
{
    public class UpdateOrderRequest
    {
        public string? ShippingUnit { get; set; }

        public string? ShippingCode { get; set; }
        public decimal? DeliveredFee { get; set; }
        public string? Status { get; set; }
    }
}
