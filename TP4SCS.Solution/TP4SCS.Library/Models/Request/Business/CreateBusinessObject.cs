using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TP4SCS.Library.Models.Request.Branch;

namespace TP4SCS.Library.Models.Request.BusinessProfile
{
    public class CreateBusinessObject
    {
        [Required]
        public int OwnerId { get; set; }

        [Required]
        [DefaultValue("string")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(10, MinimumLength = 10)]
        [DefaultValue("string")]
        public string Phone { get; set; } = string.Empty;
    }
}
