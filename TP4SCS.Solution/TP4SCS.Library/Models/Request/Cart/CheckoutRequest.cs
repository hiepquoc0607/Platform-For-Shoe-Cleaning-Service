using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Cart
{
    public class CheckoutRequest
    {
        public int[] CartItemIds { get; set; } = Array.Empty<int>();

        public int AccountId { get; set; }

        public int? AddressId { get; set; }

        [DefaultValue(false)]
        public bool IsAutoReject { get; set; }

        [DefaultValue("string")]
        public string? Note { get; set; }

        public decimal DeliveredFee { get; set; }

        [DefaultValue("string")]
        public string? ShippingUnit { get; set; }

        [DefaultValue("string")]
        public string? ShippingCode { get; set; }
    }
}
