using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IServiceCategoryRepository _categoryRepository;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper, IServiceCategoryRepository categoryRepository)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task AddServiceAsync(ServiceCreateRequest serviceRequest)
        {
            if (serviceRequest == null)
            {
                throw new ArgumentNullException(nameof(serviceRequest), "Service request cannot be null.");
            }

            if (serviceRequest.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }
            if(await _categoryRepository.GetCategoryByIdAsync(serviceRequest.CategoryId) == null)
            {
                throw new ArgumentException("Invalid category id.");
            }

            var services = new List<Service>();

            foreach (var branchId in serviceRequest.BranchId)
            {
                var service = _mapper.Map<Service>(serviceRequest);
                service.BranchId = branchId;
                service.CreateTime = DateTime.Now;
                service.Status = Util.UpperCaseStringStatic("active");

                services.Add(service);
            }

            await _serviceRepository.AddServiceAsync(services);
        }


        public async Task DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);

            if (service == null)
            {
                throw new Exception($"Service with ID {id} not found.");
            }

            await _serviceRepository.DeleteServiceAsync(id);
        }


        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);
            return service;
        }


        public async Task<IEnumerable<Service>?> GetServicesAsync(string? keyword = null, int pageIndex = 1, int pageSize = 5, OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Page index must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Page size must be greater than 0.");
            }

            return await _serviceRepository.GetServicesAsync(keyword, pageIndex, pageSize, orderBy);
        }




        public async Task UpdateServiceAsync(ServiceUpdateRequest serviceUpdateRequest, int existingServiceId)
        {
            if (serviceUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(serviceUpdateRequest), "Service request cannot be null.");
            }

            if (serviceUpdateRequest.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            if (serviceUpdateRequest.Rating < 0)
            {
                throw new ArgumentException("Rating cannot be negative.");
            }

            if (serviceUpdateRequest.OrderedNum < 0)
            {
                throw new ArgumentException("Ordered number cannot be negative.");
            }

            if (serviceUpdateRequest.FeedbackedNum < 0)
            {
                throw new ArgumentException("Feedbacked number cannot be negative.");
            }

            var existingService = await _serviceRepository.GetServiceByIdAsync(existingServiceId);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Service with ID {existingServiceId} not found.");
            }

            existingService.Name = serviceUpdateRequest.Name;
            existingService.CategoryId = serviceUpdateRequest.CategoryId;
            existingService.Description = serviceUpdateRequest.Description;
            existingService.Price = serviceUpdateRequest.Price;
            existingService.Rating = serviceUpdateRequest.Rating;
            existingService.Status = serviceUpdateRequest.Status;
            existingService.OrderedNum = serviceUpdateRequest.OrderedNum;
            existingService.FeedbackedNum = serviceUpdateRequest.FeedbackedNum;

            await _serviceRepository.UpdateServiceAsync(existingService);
        }


    }
}
