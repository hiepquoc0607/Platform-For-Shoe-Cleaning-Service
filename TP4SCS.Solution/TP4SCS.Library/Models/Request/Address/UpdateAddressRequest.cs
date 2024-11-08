using System.ComponentModel.DataAnnotations;

namespace TP4SCS.Library.Models.Request.Address
{
    public class UpdateAddressRequest
    {
        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{1,9}$")]
        public string WardCode { get; set; } = string.Empty;

        [Required]
        public string Ward { get; set; } = string.Empty;

        [Required]
        public int DistrictId { get; set; }

        [Required]
        public string District { get; set; } = string.Empty;

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public string Province { get; set; } = string.Empty;

    }
}
