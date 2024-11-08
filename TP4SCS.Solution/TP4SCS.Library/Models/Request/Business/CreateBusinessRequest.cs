using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.BusinessProfile
{
    public class CreateBusinessRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
