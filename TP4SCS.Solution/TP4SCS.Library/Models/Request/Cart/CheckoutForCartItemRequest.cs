using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Cart
{
    public class CheckoutForCartItemRequest
    {
        public int[] CartItemIds { get; set; } = Array.Empty<int>();

        public int AccountId { get; set; }

        public int? AddressId { get; set; }

        [DefaultValue(false)]
        public bool IsAutoReject { get; set; }

        public string? Note { get; set; }

        public bool IsShip { get; set; } = false;
    }
}
