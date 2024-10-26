namespace TP4SCS.Library.Models.Request.Service
{
    public class ServiceCreateRequest
    {
        public int[] BranchId { get; set; } = Array.Empty<int>();
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string Status { get; set; } = "Active";
    }
}
