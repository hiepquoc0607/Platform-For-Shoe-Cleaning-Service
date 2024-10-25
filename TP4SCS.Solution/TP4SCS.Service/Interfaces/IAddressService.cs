//using TP4SCS.Library.Models.Request.Address;
//using TP4SCS.Library.Models.Response.Address;
//using TP4SCS.Library.Models.Response.General;

//namespace TP4SCS.Services.Interfaces
//{
//    public interface IAddressService
//    {
//        Task<IEnumerable<AddressResponse>?> GetAddressesByAccountIdAsync(int id);

//        Task<AddressResponse?> GetAddressesByIdAsync(int id);

//        Task<int> GetAddressMaxIdAsync();

//        Task<Result> CreateAddressAsync(CreateAddressRequest createAddressRequest);

//        Task<Result> UpdateAddressAsync(int id, UpdateAddressRequest updateAddressRequest);

//        Task<Result> UpdateAddressDefaultAsync(int id);

//        Task<Result> DeleteAddressAsync(int id);

//        Task<IEnumerable<LocationResponse>> GetCityAsync();

//        Task<IEnumerable<LocationResponse>> GetProvinceByCityAsync(string city);

//        Task<IEnumerable<LocationResponse>> GetWardByProvinceAsync(string ward);
//    }
//}
