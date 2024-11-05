namespace TP4SCS.Library.Models.Request.OrderDetail
{
    public class OrderDetailCreateRequest
    {
        public int OrderId { get; set; }
        public int BranchId { get; set; }

        public int? ServiceId { get; set; }

        public int? MaterialId { get; set; }

        public int Quantity { get; set; }
    }
}
