namespace TP4SCS.Library.Models.Response.CartItem
{
    public class GroupedCartItemResponse
    {
        public int BranchId { get; set; }
        public List<CartItemResponse>? CartItemResponse { get; set; }
    }
}
