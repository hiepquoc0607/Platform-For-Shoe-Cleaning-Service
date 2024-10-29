namespace TP4SCS.Library.Models.Response.OrderDetail
{
    public class OrderDetailResponse
    {
        public int OrderId { get; set; }

        public int? ServiceId { get; set; }

        public int? MaterialId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = null!;
    }
}
