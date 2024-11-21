namespace TP4SCS.Library.Models.Response.Material
{
    public class MaterialResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }
}
