using TP4SCS.Library.Models.Response.Location;

namespace TP4SCS.Library.Models.Request.ShipFee
{
    public class GetShipFeeRequest
    {
        public int ServiceTypeId { get; set; }

        public int FromDistricId { get; set; }

        public string FromWardCode { get; set; } = string.Empty;

        public int ToDistricId { get; set; }

        public string ToWardCode { get; set; } = string.Empty;

        public int Height { get; set; }

        public int Length { get; set; }

        public int Weight { get; set; }

        public int Width { get; set; }

        public decimal InsuranceValue { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
    }
}
