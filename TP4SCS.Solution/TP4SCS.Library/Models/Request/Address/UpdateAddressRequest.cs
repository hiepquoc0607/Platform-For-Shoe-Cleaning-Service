using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Address
{
    public class UpdateAddressRequest
    {
        [DefaultValue("string")]
        public string Address { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string Ward { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string Province { get; set; } = string.Empty;

        [DefaultValue("string")]
        public string City { get; set; } = string.Empty;
    }
}
