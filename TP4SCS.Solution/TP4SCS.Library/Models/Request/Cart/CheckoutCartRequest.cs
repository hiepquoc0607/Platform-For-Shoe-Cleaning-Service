using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Cart
{
    public class CheckoutCartRequest
    {
        public List<CartCheckout> Carts { get; set; } = new List<CartCheckout>();

        public int AccountId { get; set; }

        public int? AddressId { get; set; }

        [DefaultValue(false)]
        public bool IsAutoReject { get; set; }
    }
}
