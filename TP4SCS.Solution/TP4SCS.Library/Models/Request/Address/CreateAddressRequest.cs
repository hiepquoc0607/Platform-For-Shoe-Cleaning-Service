using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Address
{
    public class CreateAddressRequest
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [DefaultValue("string")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string Ward { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string Province { get; set; } = string.Empty;

        [Required]
        [DefaultValue("string")]
        public string City { get; set; } = string.Empty;

        [Required]
        [DeniedValues(false)]
        public bool IsDefault { get; set; }
    }
}
