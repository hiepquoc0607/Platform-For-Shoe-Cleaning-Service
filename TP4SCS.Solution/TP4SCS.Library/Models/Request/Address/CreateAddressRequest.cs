using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Address
{
    public class CreateAddressRequest
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Ward { get; set; } = string.Empty;

        [Required]
        public string District { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        [DefaultValue(false)]
        public bool IsDefault { get; set; }
    }
}
