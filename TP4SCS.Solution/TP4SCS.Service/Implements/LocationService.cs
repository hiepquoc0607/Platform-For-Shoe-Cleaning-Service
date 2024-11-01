using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Location;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<LocationResponse>?>> GetCityAsync()
        {
            var cities = await _locationRepository.GetCityAsync();

            if (cities == null)
            {
                return new ApiResponse<IEnumerable<LocationResponse>?>("error", 404, "Dữ Liệu Tên Thành Phố Trống!");
            }

            var data = cities.Adapt<IEnumerable<LocationResponse>>();

            return new ApiResponse<IEnumerable<LocationResponse>?>("success", "Lấy Dữ Liệu Thành Công!", data);
        }

        public async Task<ApiResponse<IEnumerable<LocationResponse>?>> GetProviceByWardAsync(string name)
        {
            var provinces = await _locationRepository.GetProvinceByWardAsync(name);

            if (provinces == null)
            {
                return new ApiResponse<IEnumerable<LocationResponse>?>("error", 404, "Không Tìm Thấy Tên Phường!");
            }

            var data = provinces.Adapt<IEnumerable<LocationResponse>>();

            return new ApiResponse<IEnumerable<LocationResponse>?>("success", "Lấy Dữ Liệu Thành Công!", data, null);
        }

        public async Task<ApiResponse<IEnumerable<LocationResponse>?>> GetWardByCityAsync(string name)
        {
            var wards = await _locationRepository.GetWardByCityAsync(name);

            if (wards == null)
            {
                return new ApiResponse<IEnumerable<LocationResponse>?>("error", 404, "Không Tìm Thấy Tên Quận!");
            }

            var data = wards.Adapt<IEnumerable<LocationResponse>>();

            return new ApiResponse<IEnumerable<LocationResponse>?>("success", "Lấy Dữ Liệu Thành Công!", data, null);
        }
    }
}
