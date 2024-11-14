using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.SubscriptionPack;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.SubcriptionPack;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly Util _util;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper, Util util)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _util = util;
        }

        public async Task<ApiResponse<SubscriptionPackResponse>> CreatePackAsync(SubscriptionPackRequest subscriptionPackRequest)
        {
            var name = _util.FormatStringName(subscriptionPackRequest.Name);

            var isNameExisted = await _subscriptionRepository.IsPackNameExistedAsync(name);

            if (isNameExisted)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Tên Gói Đăng Kí Đã Tồn Tại!");
            }

            var newPack = _mapper.Map<SubscriptionPack>(subscriptionPackRequest);
            newPack.Name = name;
            newPack.Description = "";

            try
            {
                await _subscriptionRepository.CreatePackAsync(newPack);

                var newPck = await GetPackByIdAsync(newPack.Id);

                return new ApiResponse<SubscriptionPackResponse>("success", "Tạo Gói Đăng Kí Mới Thất Bại!", newPck.Data, 201);
            }
            catch (Exception)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Tạo Gói Đăng Kí Mới Thất Bại!");
            }
        }

        public async Task<ApiResponse<SubscriptionPackResponse?>> GetPackByIdAsync(int id)
        {
            var pack = await _subscriptionRepository.GetPackByIdAsync(id);

            if (pack == null)
            {
                return new ApiResponse<SubscriptionPackResponse?>("error", 404, "Không Tìm Thấy Thông Tin Gói Đăng Kí!");
            }

            var data = _mapper.Map<SubscriptionPackResponse>(pack);

            return new ApiResponse<SubscriptionPackResponse?>("success", "Lấy Thông Tin Gói Đăng Kí Tành Công!", data, 200);
        }

        public async Task<ApiResponse<IEnumerable<SubscriptionPackResponse>?>> GetPacksAsync()
        {
            var packs = await _subscriptionRepository.GetPacksAsync();

            if (packs == null)
            {
                return new ApiResponse<IEnumerable<SubscriptionPackResponse>?>("error", 404, "Thông Tin Gói Đăng Kí Trống!");
            }

            var data = packs.Adapt<IEnumerable<SubscriptionPackResponse>>();

            return new ApiResponse<IEnumerable<SubscriptionPackResponse>?>("success", "Lấy Thông Tin Gói Đăng Kí Tành Công!", data, 200);
        }

        public async Task<ApiResponse<SubscriptionPackResponse>> UpdatePackAsync(int id, SubscriptionPackRequest subscriptionPackRequest)
        {
            var name = _util.FormatStringName(subscriptionPackRequest.Name);

            var isNameExisted = await _subscriptionRepository.IsPackNameExistedAsync(name);

            if (isNameExisted)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Tên Gói Đăng Kí Đã Tồn Tại!");
            }

            var oldPack = await _subscriptionRepository.GetPackByIdAsync(id);

            if (oldPack == null)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 404, "Không Tìm Thấy Thông Tin Gói Đăng Kí!");
            }

            var newPack = _mapper.Map(subscriptionPackRequest, oldPack);
            newPack.Name = name;

            try
            {
                await _subscriptionRepository.UpdateAsync(newPack);

                return new ApiResponse<SubscriptionPackResponse>("success", "Cập Nhập Gói Đăng Kí Thành Công!", null, 200);
            }
            catch (Exception)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Cập Nhập Gói Đăng Kí Thất Bại!");
            }
        }
    }
}
