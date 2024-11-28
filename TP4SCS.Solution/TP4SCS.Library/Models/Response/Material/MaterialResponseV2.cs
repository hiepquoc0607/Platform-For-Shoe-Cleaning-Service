namespace TP4SCS.Library.Models.Response.Material
{
    public class MaterialResponseV2
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
