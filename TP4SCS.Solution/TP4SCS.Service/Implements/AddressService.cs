using Mapster;
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

        //Create Address
        public async Task<ApiResponse<AddressResponse>> CreateAddressAsync(CreateAddressRequest createAddressRequest)
        {
            while (createAddressRequest.IsDefault == true)
            {
                var oldDefault = await _addressRepository.GetDefaultAddressesByAccountIdAsync(createAddressRequest.AccountId);

                if (oldDefault != null)
                {
                    oldDefault.IsDefault = false;

                    try
                    {
                        await _addressRepository.UpdateAddressAsync(oldDefault);
                    }
                    catch (Exception)
                    {
                        return new ApiResponse<AddressResponse>("error", 400, "Tạo Địa Chỉ Thất Bại!");
                    }
                }
            }

            var newAddress = _mapper.Map<AccountAddress>(createAddressRequest);

            try
            {
                await _addressRepository.CreateAddressAsync(newAddress);

                var newId = await GetAddressMaxIdAsync();

                var newAddr = await GetAddressesByIdAsync(newId);

                return new ApiResponse<AddressResponse>("success", "Tạo Địa Chỉ Thành Công!", newAddr.Data);
            }
            catch (Exception)
            {
                return new ApiResponse<AddressResponse>("error", 400, "Tạo Địa Chỉ Thất Bại!");
            }
        }

        //Delete Address
        public async Task<ApiResponse<AddressResponse>> DeleteAddressAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByIdAsync(id);

            if (address == null)
            {
                return new ApiResponse<AddressResponse>("error", 404, "Địa Chỉ Không Tồn Tại!");
            }

            try
            {
                await _addressRepository.DeletAddressAsync(id);

                return new ApiResponse<AddressResponse>("success", "Xoá Địa Chỉ Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AddressResponse>("success", 400, "Xoá Địa Chỉ Thất Bại!");
            }
        }

        //Get Address By Account Id
        public async Task<ApiResponse<IEnumerable<AddressResponse>?>> GetAddressesByAccountIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByAccountIdAsync(id);

            if (address == null)
            {
                return new ApiResponse<IEnumerable<AddressResponse>?>("error", 404, "Tài Khoản Không Có Địa Chỉ!");
            }

            var data = address.Adapt<IEnumerable<AddressResponse>>();

            return new ApiResponse<IEnumerable<AddressResponse>?>("success", "Lấy Địa Chỉ Thành Công!", data);
        }

        //Get Address By Id
        public async Task<ApiResponse<AddressResponse?>> GetAddressesByIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByIdAsync(id);

            if (address == null)
            {
                return new ApiResponse<AddressResponse?>("error", 404, "Địa Chỉ Không Tồn Tại!");
            }

            var data = _mapper.Map<AddressResponse>(address);

            return new ApiResponse<AddressResponse?>("success", "Lấy Địa Chỉ Thành Công!", data);
        }

        //Get Address Max Id
        public async Task<int> GetAddressMaxIdAsync()
        {
            return await _addressRepository.GetAddressMaxIdAsync();
        }

        //Get City
        public Task<ApiResponse<IEnumerable<LocationResponse>>> GetCityAsync()
        {
            throw new NotImplementedException();
        }

        //Get Province By City
        public Task<ApiResponse<IEnumerable<LocationResponse>>> GetProvinceByCityAsync(string city)
        {
            throw new NotImplementedException();
        }

        //Get Ward By Province
        public Task<ApiResponse<IEnumerable<LocationResponse>>> GetWardByProvinceAsync(string ward)
        {
            throw new NotImplementedException();
        }

        //Update Address
        public async Task<ApiResponse<AddressResponse>> UpdateAddressAsync(int id, UpdateAddressRequest updateAddressRequest)
        {
            var oldAddress = await _addressRepository.GetAddressesByIdAsync(id);

            if (oldAddress == null)
            {
                return new ApiResponse<AddressResponse>("error", 404, "Địa Chỉ Không Tồn Tại!");
            }

            var newAddress = _mapper.Map(updateAddressRequest, oldAddress);

            try
            {
                await _addressRepository.UpdateAddressAsync(newAddress);

                return new ApiResponse<AddressResponse>("success", "Cập Nhập Địa Chỉ Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AddressResponse>("error", 400, "Cập Nhập Địa Chỉ Thất Bại!");
            }
        }

        //Update Address To Default
        public async Task<ApiResponse<AddressResponse>> UpdateAddressDefaultAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByIdAsync(id);

            if (address == null)
            {
                return new ApiResponse<AddressResponse>("error", 404, "Địa Chỉ Không Tồn Tại!");
            }

            var oldDefault = await _addressRepository.GetDefaultAddressesByAccountIdAsync(address.AccountId);

            if (oldDefault != null)
            {
                address.IsDefault = true;
                oldDefault.IsDefault = false;
            }

            address.IsDefault = true;

            try
            {
                await _addressRepository.UpdateAddressAsync(address);

                if (oldDefault != null)
                {
                    await _addressRepository.UpdateAddressAsync(oldDefault);
                }

                return new ApiResponse<AddressResponse>("success", " Đổi Địa Chỉ Mặc Định Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AddressResponse>("error", 400, " Đổi Địa Chỉ Mặc Định Thất Bại!"); ;
            }
        }
    }
}
