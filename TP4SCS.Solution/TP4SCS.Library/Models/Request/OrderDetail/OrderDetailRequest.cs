using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.OrderDetail
{
    public class OrderDetailRequest
    {
        public int OrderId { get; set; }

        public int? ServiceId { get; set; }

        public int? MaterialId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
