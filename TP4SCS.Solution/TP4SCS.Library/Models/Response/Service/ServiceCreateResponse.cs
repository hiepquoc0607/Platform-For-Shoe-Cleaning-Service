namespace TP4SCS.Library.Models.Response.Service
{
    public class ServiceCreateResponse
    {
        public int Id { get; set; }

        public required int[] BranchId { get; set; }
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = null!;
    }
}
