using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Utils;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IServiceCategoryRepository _categoryRepository;
        private readonly IPromotionService _promotionService;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper, IServiceCategoryRepository categoryRepository
            , IPromotionService promotionService)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _promotionService = promotionService;
        }

        public async Task AddServiceAsync(ServiceCreateRequest serviceRequest)
        {
            if (serviceRequest == null)
            {
                throw new ArgumentNullException(nameof(serviceRequest), "Yêu cầu dịch vụ không được để trống.");
            }

            if (serviceRequest.Price <= 0)
            {
                throw new ArgumentException("Giá phải lớn hơn 0.");
            }

            if (serviceRequest.NewPrice.HasValue && serviceRequest.NewPrice <= 0 && serviceRequest.NewPrice.HasValue)
            {
                throw new ArgumentException("Giá giảm phải lớn hơn 0.");
            }

            if (serviceRequest.NewPrice.HasValue && serviceRequest.NewPrice >= serviceRequest.Price && serviceRequest.NewPrice.HasValue)
            {
                throw new ArgumentException("Giá giảm phải bé hơn giá gốc.");
            }

            var services = new List<Service>();

            foreach (var branchId in serviceRequest.BranchId)
            {
                var service = _mapper.Map<Service>(serviceRequest);
                service.BranchId = branchId;
                service.CreateTime = DateTime.Now;

                if (serviceRequest.NewPrice.HasValue)
                {
                    service.Promotion = new Promotion
                    {
                        NewPrice = serviceRequest.NewPrice.Value,
                        SaleOff = 100 - (int)Math.Round((serviceRequest.NewPrice.Value / serviceRequest.Price * 100),
                        MidpointRounding.AwayFromZero),
                        Status = StatusConstants.Available.ToUpper()
                    };
                }
                else
                {
                    service.Promotion = null;
                }

                services.Add(service);
            }

            await _serviceRepository.AddServiceAsync(services);
        }



        public async Task DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);

            if (service == null)
            {
                throw new Exception($"Dịch vụ với ID {id} không tìm thấy.");
            }

            if (service.Promotion != null)
            {
                await _promotionService.DeletePromotionAsync(service.Promotion.Id);
            }

            await _serviceRepository.DeleteServiceAsync(id);
        }


        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);
            return service;
        }


        public async Task<IEnumerable<Service>?> GetServicesAsync(string? keyword = null,
            string? status = null, int pageIndex = 1, int pageSize = 5, OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Chỉ số trang phải lớn hơn 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Kích thước trang phải lớn hơn 0.");
            }

            return await _serviceRepository.GetServicesAsync(keyword, status, pageIndex, pageSize, orderBy);
        }


        public async Task UpdateServiceAsync(ServiceUpdateRequest serviceUpdateRequest, int existingServiceId)
        {
            if (serviceUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(serviceUpdateRequest), "Yêu cầu dịch vụ không được để trống.");
            }

            if (serviceUpdateRequest.Price <= 0)
            {
                throw new ArgumentException("Giá phải lớn hơn 0.");
            }

            if (serviceUpdateRequest.Rating < 0)
            {
                throw new ArgumentException("Đánh giá không được âm.");
            }

            if (serviceUpdateRequest.OrderedNum < 0)
            {
                throw new ArgumentException("Số lượng đã đặt không được âm.");
            }
            if (string.IsNullOrEmpty(serviceUpdateRequest.Status))
            {
                throw new ArgumentException("Status không được rỗng.");
            }
            if (serviceUpdateRequest.FeedbackedNum < 0)
            {
                throw new ArgumentException("Số lượng phản hồi không được âm.");
            }
            var category = await _categoryRepository.GetCategoryByIdAsync(serviceUpdateRequest.CategoryId);

            if (category == null)
            {
                throw new ArgumentException("ID danh mục không hợp lệ.");
            }
            if (category.Status.ToUpper() == "INACTIVE")
            {
                throw new ArgumentException("Danh mục này đã ngưng hoạt động.");
            }
            var existingService = await _serviceRepository.GetServiceByIdAsync(existingServiceId);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Dịch vụ với ID {existingServiceId} không tìm thấy.");
            }

            existingService.Name = serviceUpdateRequest.Name;
            existingService.CategoryId = serviceUpdateRequest.CategoryId;
            existingService.Description = serviceUpdateRequest.Description ?? "";
            existingService.Price = serviceUpdateRequest.Price;
            existingService.Rating = serviceUpdateRequest.Rating;
            existingService.Status = serviceUpdateRequest.Status;
            existingService.OrderedNum = serviceUpdateRequest.OrderedNum;
            existingService.FeedbackedNum = serviceUpdateRequest.FeedbackedNum;
            if (serviceUpdateRequest.PromotionUpdateRequest != null &&
                serviceUpdateRequest.PromotionUpdateRequest.NewPrice < serviceUpdateRequest.Price)
            {
                if (existingService.Promotion != null)
                {
                    existingService.Promotion.NewPrice = serviceUpdateRequest.PromotionUpdateRequest.NewPrice;
                    existingService.Promotion.Status = Util.UpperCaseStringStatic(serviceUpdateRequest.PromotionUpdateRequest.Status);

                    await _promotionService.UpdatePromotionAsync(existingService.Promotion, existingService.Promotion.Id);
                }
                else
                {
                    var newPromotion = new Promotion
                    {
                        ServiceId = existingServiceId,
                        NewPrice = serviceUpdateRequest.PromotionUpdateRequest.NewPrice,
                        Status = Util.UpperCaseStringStatic(serviceUpdateRequest.Status)
                    };
                    await _promotionService.AddPromotionAsync(newPromotion);
                }
            }
            else if (existingService.Promotion != null)
            {
                await _promotionService.DeletePromotionAsync(existingService.Promotion.Id);
                existingService.Promotion = null;
            }

            await _serviceRepository.UpdateServiceAsync(existingService);
        }

        public async Task<IEnumerable<Service>?> GetDiscountedServicesAsync()
        {
            var services = await _serviceRepository.GetServicesAsync(null, null);

            var discountedServices = services?.Where(service =>
                service.Promotion != null &&
                service.Promotion.Status.ToUpper() == StatusConstants.Available.ToUpper()
            );

            return discountedServices;
        }

        public Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null)
        {
            return _serviceRepository.GetTotalServiceCountAsync(keyword, status);
        }

        public async Task<decimal> GetServiceFinalPriceAsync(int serviceId)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
            if (service == null)
            {
                throw new KeyNotFoundException($"Dịch vụ với ID {serviceId} không tìm thấy.");
            }

            if (service.Promotion == null)
            {
                return service.Price;
            }

            var isPromotionActive = await _promotionService.IsPromotionActiveAsync(service.Promotion.Id);
            if (!isPromotionActive)
            {
                return service.Price;
            }

            decimal finalPrice = service.Price * (1 - (decimal)service.Promotion.SaleOff / 100);

            return finalPrice;
        }

    }
}
