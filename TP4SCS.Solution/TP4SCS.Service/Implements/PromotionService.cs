using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IServiceRepository _serviceRepository;

        public PromotionService(IPromotionRepository promotionRepository, IServiceRepository serviceRepository)
        {
            _promotionRepository = promotionRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task AddPromotionAsync(Promotion promotion)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion), "Yêu cầu khuyến mãi không được để trống.");
            }

            if (promotion.SaleOff < 0 || promotion.SaleOff > 100)
            {
                throw new ArgumentException("Giảm giá phải nằm trong khoảng từ 0 đến 100%.");
            }

            if (promotion.StartTime < DateTime.Now.AddDays(1))
            {
                throw new ArgumentException("Ngày bắt đầu phải lớn hơn hoặc bằng ngày hôm sau.");
            }

            if (promotion.EndTime <= promotion.StartTime)
            {
                throw new ArgumentException("Ngày kết thúc phải lớn hơn ngày bắt đầu.");
            }

            var service = await _serviceRepository.GetServiceByIdAsync(promotion.ServiceId);
            if (service == null)
            {
                throw new ArgumentException("ID dịch vụ không hợp lệ.");
            }
            if (service.Status.ToUpper() == StatusConstants.Inactive)
            {
                throw new ArgumentException("Dịch vụ này đã ngưng hoạt động.");
            }

            promotion.Status = Util.UpperCaseStringStatic(promotion.Status);
            promotion.NewPrice = service.Price * (1 - promotion.SaleOff / 100);

            await _promotionRepository.AddPromotionAsync(promotion);
        }

        public async Task DeletePromotionAsync(int id)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(id);

            if (promotion == null)
            {
                throw new Exception($"Khuyến mãi với ID {id} không tìm thấy.");
            }

            await _promotionRepository.DeletePromotionAsync(id);
        }

        public async Task<Promotion?> GetPromotionByIdAsync(int id)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(id);
            return promotion;
        }

        public async Task<IEnumerable<Promotion>?> GetPromotionsAsync(string? keyword = null, string? status = null, int pageIndex = 1, int pageSize = 5, OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Chỉ số trang phải lớn hơn 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Kích thước trang phải lớn hơn 0.");
            }

            return await _promotionRepository.GetPromotionsAsync(keyword, status, pageIndex, pageSize, orderBy);
        }

        public async Task UpdatePromotionAsync(Promotion promotion, int existingPromotionId)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion), "Yêu cầu cập nhật khuyến mãi không được để trống.");
            }

            if (promotion.SaleOff < 0 || promotion.SaleOff > 100)
            {
                throw new ArgumentException("Giảm giá phải nằm trong khoảng từ 0 đến 100%.");
            }

            if (promotion.StartTime < DateTime.Now.AddDays(1))
            {
                throw new ArgumentException("Ngày bắt đầu phải lớn hơn hoặc bằng ngày hôm sau.");
            }

            if (promotion.EndTime <= promotion.StartTime)
            {
                throw new ArgumentException("Ngày kết thúc phải lớn hơn ngày bắt đầu.");
            }

            var service = await _serviceRepository.GetServiceByIdAsync(promotion.ServiceId);
            if (service == null)
            {
                throw new ArgumentException("ID dịch vụ không hợp lệ.");
            }

            var existingPromotion = await _promotionRepository.GetPromotionByIdAsync(existingPromotionId);
            if (existingPromotion == null)
            {
                throw new KeyNotFoundException($"Khuyến mãi với ID {existingPromotionId} không tìm thấy.");
            }

            existingPromotion.SaleOff = promotion.SaleOff;
            existingPromotion.NewPrice = service.Price * (1 - promotion.SaleOff / 100);
            existingPromotion.StartTime = promotion.StartTime;
            existingPromotion.EndTime = promotion.EndTime;
            existingPromotion.Status = Util.UpperCaseStringStatic(promotion.Status);

            await _promotionRepository.UpdatePromotionAsync(existingPromotion);
        }
    }
}
