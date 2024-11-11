using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.BusinessProfile
{
    public class CreateBusinessRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(12), MaxLength(12)]
        [DefaultValue("string")]
        public string CitizenIdnumber { get; set; } = string.Empty;

        [Required]
        public string FrontCitizenImageUrl { get; set; } = string.Empty;

        [Required]
        public string BackCitizenImageUrl { get; set; } = string.Empty;
    }
}
