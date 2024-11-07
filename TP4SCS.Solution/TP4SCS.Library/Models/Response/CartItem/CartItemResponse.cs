namespace TP4SCS.Library.Models.Response.CartItem
{
    public class CartItemResponse
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public int BranchId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceStatus { get; set; } = null!;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
