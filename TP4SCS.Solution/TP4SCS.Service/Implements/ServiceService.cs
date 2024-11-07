using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.AssetUrl;
using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.BranchService;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Promotion;
using TP4SCS.Library.Models.Response.Service;
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
        private readonly IAssetUrlService _assetUrlService;

        public ServiceService(IServiceRepository serviceRepository,
            IMapper mapper,
            IServiceCategoryRepository categoryRepository,
            IPromotionService promotionService,
            IAssetUrlService assetUrlService)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _promotionService = promotionService;
            _assetUrlService = assetUrlService;
        }

        public async Task AddServiceAsync(ServiceCreateRequest serviceRequest, int businessId)
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

            if (serviceRequest.NewPrice.HasValue && serviceRequest.NewPrice > serviceRequest.Price)
            {
                throw new ArgumentException("Giá sau khi giảm phải bé hơn hoặc bằng giá gốc.");
            }
            if (serviceRequest.AssetUrls == null)
            {
                throw new ArgumentException("Hình ảnh không được để trống.");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(serviceRequest.CategoryId);

            if (category == null)
            {
                throw new ArgumentException("ID danh mục không hợp lệ.");
            }

            if (Util.IsEqual(category.Status, StatusConstants.Unavailable))
            {
                throw new ArgumentException("Danh mục này đã ngưng hoạt động.");
            }


            var service = _mapper.Map<Service>(serviceRequest);
            service.CreateTime = DateTime.Now;

            if (serviceRequest.NewPrice.HasValue)
            {
                service.Promotion = new Promotion
                {
                    NewPrice = serviceRequest.NewPrice.Value,
                    SaleOff = 100 - (int)Math.Round((serviceRequest.NewPrice.Value / serviceRequest.Price * 100), MidpointRounding.AwayFromZero),
                    Status = StatusConstants.Available.ToUpper()
                };

                if (string.IsNullOrEmpty(service.Promotion.Status) || !Util.IsValidGeneralStatus(service.Promotion.Status))
                {
                    throw new ArgumentException("Status của Promotion không hợp lệ.");
                }
            }
            else
            {
                service.Promotion = null;
            }

            await _serviceRepository.AddServiceAsync(serviceRequest.BranchId, businessId, service);
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
            if (service.AssetUrls != null)
            {
                var assetUrlsToDelete = service.AssetUrls.ToList();
                foreach (var assetUrl in assetUrlsToDelete)
                {
                    await _assetUrlService.DeleteAssetUrlAsync(assetUrl.Id);
                }
            }

            await _serviceRepository.DeleteServiceAsync(id);
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);
            return service;
        }

        public async Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
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
            if (string.IsNullOrEmpty(serviceUpdateRequest.Status) || !Util.IsValidGeneralStatus(serviceUpdateRequest.Status))
            {
                throw new ArgumentException("Status của Service không hợp lệ.", nameof(serviceUpdateRequest.Status));
            }
            var category = await _categoryRepository.GetCategoryByIdAsync(serviceUpdateRequest.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("ID danh mục không hợp lệ.");
            }

            if (Util.IsEqual(category.Status, StatusConstants.INACTIVE))
            {
                throw new ArgumentException("Danh mục này đã ngưng hoạt động.");
            }
            if (serviceUpdateRequest.NewPrice.HasValue &&
                serviceUpdateRequest.NewPrice > serviceUpdateRequest.Price)
            {
                throw new ArgumentException("Giá sau khi giảm phải bé hơn hoặc bằng giá gốc.");
            }
            var existingService = await _serviceRepository.GetServiceByIdAsync(existingServiceId);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy dịch vụ nào.");
            }

            existingService.Name = serviceUpdateRequest.Name;
            existingService.CategoryId = serviceUpdateRequest.CategoryId;
            existingService.Description = serviceUpdateRequest.Description ?? "";
            existingService.Price = serviceUpdateRequest.Price;
            existingService.Status = serviceUpdateRequest.Status;

            if (serviceUpdateRequest.NewPrice.HasValue)
            {
                if (existingService.Promotion != null)
                {
                    existingService.Promotion.NewPrice = serviceUpdateRequest.NewPrice.Value;
                    existingService.Promotion.Status = StatusConstants.Available;

                    await _promotionService.UpdatePromotionAsync(existingService.Promotion, existingService.Promotion.Id);
                }
                else
                {
                    var newPromotion = new Promotion
                    {
                        ServiceId = existingService.Id,
                        NewPrice = serviceUpdateRequest.NewPrice.Value,
                        Status = StatusConstants.Available
                    };
                    await _promotionService.AddPromotionAsync(newPromotion);
                }
            }
            else if (!serviceUpdateRequest.NewPrice.HasValue)
            {
                if (existingService.Promotion != null)
                {
                    existingService.Promotion.NewPrice = existingService.Price;
                    existingService.Promotion.Status = StatusConstants.Unavailable;

                }
            }

            var existingAssetUrls = existingService.AssetUrls.ToList();
            var newAssetUrls = serviceUpdateRequest.AssetUrls;

            var newUrls = newAssetUrls.Select(a => a.Url).ToList();

            var urlsToRemove = existingService.AssetUrls.Where(a => !newUrls.Contains(a.Url)).ToList();
            if (urlsToRemove.Any())
            {
                foreach (var assetUrl in urlsToRemove)
                {
                    await _assetUrlService.DeleteAssetUrlAsync(assetUrl.Id);
                    existingService.AssetUrls.Remove(assetUrl);
                }
            }

            var urlsToAdd = newAssetUrls.Where(a => !existingAssetUrls.Any(e => e.Url == a.Url)).ToList();
            if (urlsToAdd.Any())
            {
                foreach (var newAsset in urlsToAdd)
                {
                    var newAssetUrl = new AssetUrl
                    {
                        Url = newAsset.Url,
                        IsImage = newAsset.IsImage,
                        Type = newAsset.Type
                    };
                    existingService.AssetUrls.Add(newAssetUrl);
                }
            }
            await _serviceRepository.UpdateServiceAsync(existingService, serviceUpdateRequest.BranchId);

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
        public async Task<IEnumerable<Service>?> GetServicesByBranchIdAsync(
            int branchId,
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            var service = await _serviceRepository.GetServicesAsync(keyword, status, pageIndex, pageSize, orderBy);
            return service?.Where(s => s.BranchServices.Any(bs => bs.BranchId == branchId));
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

        public async Task<ApiResponse<IEnumerable<ServiceResponse>?>> GetServiceByBusinessIdAsync(GetBusinessServiceRequest getBusinessServiceRequest)
        {
            var (services, pagination) = await _serviceRepository.GetServiceByBusinessIdAsync(getBusinessServiceRequest);

            if (services == null)
            {
                return new ApiResponse<IEnumerable<ServiceResponse>?>("error", 404, "Doanh Nghiệp Chưa Có Dịch Vụ!");
            }

            var data = services.Select(s =>
            {
                var res = _mapper.Map<ServiceResponse>(s);
                res.Status = Util.TranslateGeneralStatus(s.Status);

                if (s.Promotion != null)
                {
                    var promotionRes = _mapper.Map<PromotionResponse>(s.Promotion);
                    promotionRes.Status = Util.TranslateGeneralStatus(promotionRes.Status);
                    res.Promotion = promotionRes;
                }

                if (s.AssetUrls != null && s.AssetUrls.Any())
                {
                    var assetRes = _mapper.Map<List<AssetUrlResponse>>(s.AssetUrls);
                    res.AssetUrls = assetRes;
                }

                if (s.BranchServices != null && s.BranchServices.Any())
                {
                    var branchServiceResponses = s.BranchServices.Select(bs =>
                    {
                        var branchServiceResponse = _mapper.Map<BranchServiceResponse>(bs);

                        branchServiceResponse.Branch = _mapper.Map<BranchResponse>(bs.Branch);

                        return branchServiceResponse;
                    }).ToList();

                    res.BranchServices = branchServiceResponses;
                }

                return res;
            });

            return new ApiResponse<IEnumerable<ServiceResponse>?>("success", "Lấy Dữ Liệu Dịch Vụ Thành Công!", data, 200, pagination);
        }
    }
}