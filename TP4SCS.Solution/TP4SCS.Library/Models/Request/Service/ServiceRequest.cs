using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Service
{
    public class ServiceRequest
    {
        public int BranchId { get; set; }

        public int CategoryId { get; set; }

        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string? Description { get; set; }

        public decimal Price { get; set; }
    }
}
