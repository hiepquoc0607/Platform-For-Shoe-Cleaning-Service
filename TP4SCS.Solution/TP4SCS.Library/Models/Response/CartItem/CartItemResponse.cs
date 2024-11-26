namespace TP4SCS.Library.Models.Response.CartItem
{
    public class CartItemResponse
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public int BranchId { get; set; }
        public int ServiceId { get; set; }
        public int? MaterialId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceStatus { get; set; } = null!;
        public string? MaterialName { get; set; }
        public string? MaterialStatus { get; set; }
        public int? Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
