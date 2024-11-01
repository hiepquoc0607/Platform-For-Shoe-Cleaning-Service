namespace TP4SCS.Library.Models.Request.Material
{
    public class MaterialUpdateRequest
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Storage { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
