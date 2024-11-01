using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Location;

namespace TP4SCS.Services.Interfaces
{
    public interface ILocationService
    {
        Task<ApiResponse<IEnumerable<LocationResponse>?>> GetCityAsync();

        Task<ApiResponse<IEnumerable<LocationResponse>?>> GetWardByCityAsync(string name);

        Task<ApiResponse<IEnumerable<LocationResponse>?>> GetProviceByWardAsync(string name);
    }
}
