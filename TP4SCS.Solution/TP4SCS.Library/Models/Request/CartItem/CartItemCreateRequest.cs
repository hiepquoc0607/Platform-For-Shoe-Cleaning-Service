namespace TP4SCS.Library.Models.Request.CartItem
{
    public class CartItemCreateRequest
    {
        public int ServiceId { get; set; }
        public int BranchId { get; set; }

        public int Quantity { get; set; }
    }
}
