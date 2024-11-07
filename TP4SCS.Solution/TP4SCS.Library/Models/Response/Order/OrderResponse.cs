using TP4SCS.Library.Models.Response.OrderDetail;

namespace TP4SCS.Library.Models.Response.Order
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public int? AddressId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public bool IsAutoReject { get; set; }

        public string? Note { get; set; }

        public decimal OrderPrice { get; set; }

        public decimal DeliveredFee { get; set; }

        public decimal TotalPrice { get; set; }

        public string? ShippingUnit { get; set; }

        public string? ShippingCode { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<OrderDetailResponseV2> OrderDetails { get; set; } = new List<OrderDetailResponseV2>();
    }
}
