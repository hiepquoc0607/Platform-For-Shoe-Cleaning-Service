namespace TP4SCS.Library.Models.Response.CartItem
{
    public class CartItemResponse
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ServiceId { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
