using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Library.Utils.Utils;
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

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper, IServiceCategoryRepository categoryRepository, IPromotionService promotionService)
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

            if (serviceRequest.NewPrice.HasValue && serviceRequest.NewPrice <= 0)
            {
                throw new ArgumentException("Giá giảm phải lớn hơn 0.");
            }

            if (serviceRequest.NewPrice.HasValue && serviceRequest.NewPrice >= serviceRequest.Price)
            {
                throw new ArgumentException("Giá giảm phải bé hơn giá gốc.");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(serviceRequest.CategoryId);

            if (category == null)
            {
                throw new ArgumentException("ID danh mục không hợp lệ.");
            }

            if (Util.IsEqual(category.Status, StatusConstants.Inactive))
            {
                throw new ArgumentException("Danh mục này đã ngưng hoạt động.");
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
                        SaleOff = 100 - (int)Math.Round((serviceRequest.NewPrice.Value / serviceRequest.Price * 100), MidpointRounding.AwayFromZero),
                        Status = StatusConstants.Available.ToUpper()
                    };

                    if (string.IsNullOrEmpty(service.Promotion.Status) || !Util.IsValidPromotionStatus(service.Promotion.Status))
                    {
                        throw new ArgumentException("Status của Service không hợp lệ.");
                    }
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

        public async Task<IEnumerable<Service>?> GetServicesByBranchIdAsync(int branchId)
        {
            var services = await _serviceRepository.GetServicesAsync(null, null);
            return services.Where(s => s.BranchId == branchId);
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

            if (string.IsNullOrEmpty(serviceUpdateRequest.Status) || !Util.IsValidGeneralStatus(serviceUpdateRequest.Status))
            {
                throw new ArgumentException("Status của Service không hợp lệ.");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(serviceUpdateRequest.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("ID danh mục không hợp lệ.");
            }

            if (Util.IsEqual(category.Status, StatusConstants.Inactive))
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
            existingService.Status = serviceUpdateRequest.Status.ToUpper();

            if (serviceUpdateRequest.NewPrice.HasValue &&
                serviceUpdateRequest.NewPrice < serviceUpdateRequest.Price &&
                !string.IsNullOrEmpty(serviceUpdateRequest.PromotionStatus))
            {
                if (existingService.Promotion != null)
                {
                    existingService.Promotion.NewPrice = serviceUpdateRequest.NewPrice.Value;
                    existingService.Promotion.Status = Util.UpperCaseStringStatic(serviceUpdateRequest.PromotionStatus);

                    await _promotionService.UpdatePromotionAsync(existingService.Promotion, existingService.Promotion.Id);
                }
                else
                {
                    var newPromotion = new Promotion
                    {
                        ServiceId = existingServiceId,
                        NewPrice = serviceUpdateRequest.NewPrice.Value,
                        Status = Util.UpperCaseStringStatic(serviceUpdateRequest.PromotionStatus)
                    };
                    await _promotionService.AddPromotionAsync(newPromotion);
                }
            }
            else if (!serviceUpdateRequest.NewPrice.HasValue && string.IsNullOrEmpty(serviceUpdateRequest.PromotionStatus))
            {
                if (existingService.Promotion != null)
                {
                    await _promotionService.DeletePromotionAsync(existingService.Promotion.Id);
                    existingService.Promotion = null;
                }
            }

            await _serviceRepository.UpdateServiceAsync(existingService);
        }


        public async Task<IEnumerable<Service>?> GetDiscountedServicesAsync()
        {
            var services = await _serviceRepository.GetServicesAsync(null, null);

            var discountedServices = services?.Where(service =>
                service.Promotion != null &&
                Util.IsEqual(service.Promotion.Status, StatusConstants.Available)
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

            var isPromotionActive = Util.IsEqual(service.Promotion.Status, StatusConstants.Available);
            if (!isPromotionActive)
            {
                return service.Price;
            }

            decimal finalPrice = service.Promotion.NewPrice;

            return finalPrice;
        }
    }
}
