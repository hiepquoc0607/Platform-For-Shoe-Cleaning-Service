using TP4SCS.Library.Models.Request.Address;
using TP4SCS.Library.Models.Response.Address;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Result<IEnumerable<AddressResponse>?>> GetAddressesByAccountIdAsync(int id);

        Task<Result<AddressResponse?>> GetAddressesByIdAsync(int id);

        Task<int> GetAddressMaxIdAsync();

        Task<Result<AddressResponse>> CreateAddressAsync(CreateAddressRequest createAddressRequest);

        Task<Result<AddressResponse>> UpdateAddressAsync(int id, UpdateAddressRequest updateAddressRequest);

        Task<Result<AddressResponse>> UpdateAddressDefaultAsync(int id);

        Task<Result<AddressResponse>> DeleteAddressAsync(int id);

        Task<Result<IEnumerable<LocationResponse>>> GetCityAsync();

        Task<Result<IEnumerable<LocationResponse>>> GetProvinceByCityAsync(string city);

        Task<Result<IEnumerable<LocationResponse>>> GetWardByProvinceAsync(string ward);
    }
}
