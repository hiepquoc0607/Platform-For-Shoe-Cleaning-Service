namespace TP4SCS.Library.Models.Request.Cart
{
    public class CartCheckout
    {
        public int[] CartItemIds { get; set; } = Array.Empty<int>();
        public string? Note { get; set; }
        public bool IsShip { get; set; } = false;
    }
}
