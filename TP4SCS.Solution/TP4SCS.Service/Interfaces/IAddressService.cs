using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Address;
using TP4SCS.Library.Models.Response.Address;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AccountAddress>> GetAddressesByAccountIdAsync(int id);

        Task<int> GetAddressMaxIdAsync();

        Task<Result> CreateAddressAsync(int id, CreateAddressRequest createAddressRequest);

        Task<Result> UpdateAddressAsync(int id, UpdateAddressRequest updateAddressRequest);

        Task<Result> UpdateAddressDefaultAsync(int id);

        Task<Result> DeleteAddressAsync(int id);

        Task<IEnumerable<CityResponse>> GetCityAsync();
    }
}
