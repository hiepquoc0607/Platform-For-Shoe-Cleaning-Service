using TP4SCS.Library.Models.Request.ShipFee;
using TP4SCS.Library.Models.Response.Location;

namespace TP4SCS.Services.Interfaces
{
    public interface IShipService
    {
        Task<List<AvailableService>> GetAvailableServicesAsync(HttpClient httpClient, int fromDistrict, int toDistrict);

        Task<List<District>> GetDistrictsAsync(HttpClient httpClient, int provinceId);

        Task<List<Province>> GetProvincesAsync(HttpClient httpClient);

        Task<decimal> GetShippingFeeAsync(HttpClient httpClient, GetShipFeeRequest getShipFeeRequest);

        Task<List<Ward>> GetWardsAsync(HttpClient httpClient, int districtId);
    }
}
