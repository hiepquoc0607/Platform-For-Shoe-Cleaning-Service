using System.ComponentModel;
using TP4SCS.Library.Models.Request.CartItem;

namespace TP4SCS.Library.Models.Request.Cart
{
    public class CheckoutForServiceRequest
    {
        public List<CartItemCreateRequest> Items { get; set; } = new List<CartItemCreateRequest> { };

        public int AccountId { get; set; }

        public int? AddressId { get; set; }

        [DefaultValue(false)]
        public bool IsAutoReject { get; set; }

        public string? Note { get; set; }

        public bool IsShip { get; set; } = false;
    }
}
