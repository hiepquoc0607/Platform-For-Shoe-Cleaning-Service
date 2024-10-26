namespace TP4SCS.Library.Models.Request.Material
{
    public class MaterialUpdateRequest
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Storage { get; set; }

        public string Status { get; set; } = "Hoạt động";
    }
}
