namespace TP4SCS.Library.Models.Request.Address
{
    public class UpdateAddressRequest
    {
        public string Address { get; set; } = string.Empty;

        public string Ward { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;
    }
}
