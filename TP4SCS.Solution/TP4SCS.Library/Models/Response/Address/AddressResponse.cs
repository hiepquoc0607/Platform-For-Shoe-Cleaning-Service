namespace TP4SCS.Library.Models.Response.Address
{
    public class AddressResponse
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Ward { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
