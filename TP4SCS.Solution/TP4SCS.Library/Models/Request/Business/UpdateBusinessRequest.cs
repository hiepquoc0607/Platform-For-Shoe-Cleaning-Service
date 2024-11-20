using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Business
{
    public class UpdateBusinessRequest
    {
        public string Name { get; set; } = string.Empty;

        [Phone]
        [StringLength(10, MinimumLength = 10)]
        [DefaultValue("string")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MinLength(12), MaxLength(12)]
        [DefaultValue("string")]
        public string CitizenId { get; set; } = string.Empty;

        public string FrontCitizenImageUrl { get; set; } = string.Empty;

        public string BackCitizenImageUrl { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
