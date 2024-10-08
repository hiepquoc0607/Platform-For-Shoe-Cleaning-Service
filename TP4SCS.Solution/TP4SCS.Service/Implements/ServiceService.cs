using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request;
using TP4SCS.Library.Models.Response;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task AddService(ServiceRequest serviceRequest)
        {
            if (serviceRequest == null)
            {
                throw new ArgumentNullException(nameof(serviceRequest), "Service request cannot be null.");
            }

            if (serviceRequest.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            if (serviceRequest.Rating < 0)
            {
                throw new ArgumentException("Rating cannot be negative.");
            }

            if (serviceRequest.OrderedNum < 0)
            {
                throw new ArgumentException("Ordered number cannot be negative.");
            }

            if (serviceRequest.FeedbackedNum < 0)
            {
                throw new ArgumentException("Feedbacked number cannot be negative.");
            }

            var service = _mapper.Map<Service>(serviceRequest);
            service.CreateTime = DateTime.Now;

            await _serviceRepository.AddService(service);
        }


        public async Task DeleteService(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);

            if (service == null)
            {
                throw new Exception($"Service with ID {id} not found.");
            }

            await _serviceRepository.DeleteService(id);
        }


        public async Task<Service> GetServiceById(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            return service;
        }


        public async Task<IEnumerable<Service>> GetServices(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name")
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Page index must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Page size must be greater than 0.");
            }

            return await _serviceRepository.GetServices(keyword, pageIndex, pageSize, orderBy);
        }




        public async Task UpdateService(ServiceUpdateRequest serviceUpdateRequest, int existingServiceId)
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

            var existingService = await _serviceRepository.GetServiceById(existingServiceId);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Service with ID {existingServiceId} not found.");
            }

            existingService.Name = serviceUpdateRequest.Name;
            existingService.Description = serviceUpdateRequest.Description;
            existingService.Price = serviceUpdateRequest.Price;
            existingService.Rating = serviceUpdateRequest.Rating;
            existingService.Status = serviceUpdateRequest.Status;
            existingService.OrderedNum = serviceUpdateRequest.OrderedNum;
            existingService.FeedbackedNum = serviceUpdateRequest.FeedbackedNum;

            await _serviceRepository.UpdateService(existingService);
        }


    }
}
