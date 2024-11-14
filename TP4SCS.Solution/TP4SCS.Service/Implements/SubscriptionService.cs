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
            int totalPack = await _subscriptionRepository.CountPackAsync();

            if (totalPack > 4)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Đã Có Tối Đa 3 Gói Đăng Kí!");
            }

            List<int> periods = await _subscriptionRepository.GetPeriodArrayAsync();

            if (periods.Contains(subscriptionPackRequest.Period))
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Thời Hạn Gói Đăng Kí Trùng Lập!");
            }

            var name = _util.FormatStringName(subscriptionPackRequest.Name);

            var isNameExisted = await _subscriptionRepository.IsPackNameExistedAsync(name);

            if (isNameExisted)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Tên Gói Đăng Kí Đã Tồn Tại!");
            }

            periods.Add(subscriptionPackRequest.Period);
            periods.Sort();

            int newIndex = periods.IndexOf(subscriptionPackRequest.Period);

            string description = "";

            if (newIndex != 0)
            {
                decimal basePrice = await _subscriptionRepository.GetPackPriceByPeriodAsync(periods[0]);
                decimal savePrice = (basePrice / periods[0]) - (subscriptionPackRequest.Price / subscriptionPackRequest.Period);
                string save = savePrice.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture);

                if (savePrice > 0)
                {
                    description = $"Tiết Kiệm " + save + "đ/Tháng";
                }
            }

            var newPack = _mapper.Map<SubscriptionPack>(subscriptionPackRequest);
            newPack.Name = name;
            newPack.Description = description;

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
            var oldPack = await _subscriptionRepository.GetPackByIdAsync(id);

            if (oldPack == null)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 404, "Không Tìm Thấy Thông Tin Gói Đăng Kí!");
            }

            List<int> periods = await _subscriptionRepository.GetPeriodArrayAsync();

            if (oldPack.Period != subscriptionPackRequest.Period && periods.Contains(subscriptionPackRequest.Period))
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Thời Hạn Gói Đăng Kí Trùng Lập!");
            }

            var name = _util.FormatStringName(subscriptionPackRequest.Name);

            var isNameExisted = await _subscriptionRepository.IsPackNameExistedAsync(name);

            if (!oldPack.Name.Equals(name) && isNameExisted)
            {
                return new ApiResponse<SubscriptionPackResponse>("error", 400, "Tên Gói Đăng Kí Đã Tồn Tại!");
            }

            periods.Add(subscriptionPackRequest.Period);
            periods.Sort();

            int newIndex = periods.IndexOf(subscriptionPackRequest.Period);

            string description = "";

            if (newIndex != 0)
            {
                decimal basePrice = await _subscriptionRepository.GetPackPriceByPeriodAsync(periods[0]);
                decimal savePrice = (basePrice / periods[0]) - (subscriptionPackRequest.Price / subscriptionPackRequest.Period);
                string save = savePrice.ToString("#,0", System.Globalization.CultureInfo.InvariantCulture);

                if (savePrice > 0)
                {
                    description = $"Tiết Kiệm " + save + "đ/Tháng";
                }
            }

            var newPack = _mapper.Map(subscriptionPackRequest, oldPack);
            newPack.Name = name;
            newPack.Description = description;

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
