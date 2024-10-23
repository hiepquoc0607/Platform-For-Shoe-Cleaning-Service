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

        public async Task<Result> CreateAddressAsync(CreateAddressRequest createAddressRequest)
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
                        return new Result { IsSuccess = false, StatusCode = 400, Message = "Tạo địa chỉ thất bại!" };
                    }
                }
            }

            var newAddress = _mapper.Map<AccountAddress>(createAddressRequest);

            try
            {
                await _addressRepository.CreateAddressAsync(newAddress);

                return new Result { IsSuccess = true, Message = "Tạo địa chỉ thành công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Tạo địa chỉ thất bại!" };
            }
        }

        public async Task<Result> DeleteAddressAsync(int id)
        {
            try
            {
                await _addressRepository.DeletAddressAsync(id);

                return new Result { IsSuccess = true, Message = "Xoá địa chỉ thành công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Xoá địa chỉ thất bại!" };
            }
        }

        public async Task<IEnumerable<AddressResponse>?> GetAddressesByAccountIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByAccountIdAsync(id);

            if (address == null)
            {
                return null;
            }

            var result = address.Adapt<IEnumerable<AddressResponse>>();

            return result;
        }

        public async Task<AddressResponse?> GetAddressesByIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByIdAsync(id);

            if (address == null)
            {
                return null;
            }

            var result = _mapper.Map<AddressResponse>(address);

            return result;
        }

        public async Task<int> GetAddressMaxIdAsync()
        {
            return await _addressRepository.GetAddressMaxIdAsync();
        }

        public Task<IEnumerable<LocationResponse>> GetCityAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocationResponse>> GetProvinceByCityAsync(string city)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocationResponse>> GetWardByProvinceAsync(string ward)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateAddressAsync(int id, UpdateAddressRequest updateAddressRequest)
        {
            var oldAddress = await _addressRepository.GetAddressesByIdAsync(id);

            if (oldAddress == null)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Địa chỉ không tồn tại!" };
            }

            var newAddress = _mapper.Map(updateAddressRequest, oldAddress);

            try
            {
                await _addressRepository.UpdateAddressAsync(newAddress);

                return new Result { IsSuccess = true, Message = "Cập nhập địa chỉ thành công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Cập nhập địa chỉ thát bại!" };
            }
        }

        public async Task<Result> UpdateAddressDefaultAsync(int id)
        {
            var address = await _addressRepository.GetAddressesByIdAsync(id);

            if (address == null)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Địa chỉ không tồn tại!" };
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

                return new Result { IsSuccess = true, Message = "Đổi địa chỉ mặc định thành công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Đổi địa chỉ mặc định thành công!" };
            }
        }
    }
}
