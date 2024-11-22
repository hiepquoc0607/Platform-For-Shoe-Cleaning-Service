namespace TP4SCS.Library.Models.Response.CartItem
{
    public class GroupCartItemByServiceResponse
    {
        public int ServiceId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceStatus { get; set; } = string.Empty;
        public List<CartItemResponse> Materials { get; set; } = new();
    }
}
