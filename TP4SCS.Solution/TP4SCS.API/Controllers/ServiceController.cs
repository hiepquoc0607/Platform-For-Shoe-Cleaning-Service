using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request;
using TP4SCS.Library.Models.Response;
using TP4SCS.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TP4SCS.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceService _serviceService;

        public ServiceController(IMapper mapper, IServiceService serviceService)
        {
            _mapper = mapper;
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServices(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name")
        {
            var services = await _serviceService.GetServices(keyword, pageIndex, pageSize, orderBy);
            var totalServices = await _serviceService.GetServices(keyword);
            var totalCount = services.Count();

            var pagedResponse = new PagedResponse<ServiceResponse>(
                services.Select(s => _mapper.Map<ServiceResponse>(s)),
                totalCount,
                pageIndex,
                pageSize
            );

            return Ok(new ResponseObject<PagedResponse<ServiceResponse>>("Fetch Service Success", pagedResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            try
            {
                var service = await _serviceService.GetServiceById(id);
                if (service == null)
                {
                    Ok(new ResponseObject<ServiceResponse>($"Service with ID {id} not found.", null));
                }
                var response = _mapper.Map<ServiceResponse>(service);
                return Ok(new ResponseObject<ServiceResponse>("Fetch Service Success", response));
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceRequest request)
        {
            try
            {
                await _serviceService.AddService(request);

                var serviceResponse = _mapper.Map<ServiceResponse>(_mapper.Map<Service>(request));
                return Ok(new ResponseObject<ServiceResponse>("Create Service Success", serviceResponse));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<ServiceResponse>($"Error: {ex.Message}", null));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceResponse>($"Validation Error: {ex.Message}", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceResponse>($"An unexpected error occurred: {ex.Message}", null));
            }
        }

        [HttpPut("{existingServiceId}")]
        public async Task<IActionResult> UpdateService(int existingServiceId, [FromBody] ServiceUpdateRequest request)
        {
            try
            {
                await _serviceService.UpdateService(request, existingServiceId);
                var service = await _serviceService.GetServiceById(existingServiceId);
                return Ok(new ResponseObject<ServiceResponse>("Update Service Success",
                    _mapper.Map<ServiceResponse>(service)));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<ServiceResponse>(ex.Message));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<ServiceResponse>(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceResponse>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceResponse>($"An error occurred: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                await _serviceService.DeleteService(id);
                return Ok(new ResponseObject<ServiceResponse>("Delete Service Success", null));
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<ServiceResponse>(ex.Message, null));
            }
        }


    }
}
