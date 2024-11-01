using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Category
{
    public class ServiceCategoryRequest
    {
        [Required]
        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;
    }
}
