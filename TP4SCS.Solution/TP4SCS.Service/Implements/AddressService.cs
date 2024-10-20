using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Address;
using TP4SCS.Library.Models.Response.Address;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly Util _util;

        public AddressService(IAddressRepository addressRepository, IMapper mapper, Util util)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _util = util;
        }

        public Task<Result> CreateAddressAsync(int id, CreateAddressRequest createAddressRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAddressAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AccountAddress>> GetAddressesByAccountIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAddressMaxIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CityResponse>> GetCityAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAddressAsync(int id, UpdateAddressRequest updateAddressRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAddressDefaultAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
