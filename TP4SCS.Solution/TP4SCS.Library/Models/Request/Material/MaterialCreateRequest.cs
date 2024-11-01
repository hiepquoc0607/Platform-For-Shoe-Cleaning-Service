using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Material
{
    public class MaterialCreateRequest
    {
        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Storage { get; set; }

        [DefaultValue("string")]
        public string Status { get; set; } = string.Empty;
    }
}
