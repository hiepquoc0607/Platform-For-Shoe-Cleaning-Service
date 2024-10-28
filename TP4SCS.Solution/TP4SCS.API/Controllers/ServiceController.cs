using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Promotion;
using TP4SCS.Library.Models.Response.Service;
using TP4SCS.Library.Utils;
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
        public async Task<IActionResult> GetServicesAync([FromQuery] PagedRequest pagedRequest)
        {
            var services = await _serviceService.GetServicesAsync(pagedRequest.Keyword,
                Util.TranslateGeneralStatus(pagedRequest.Status),
                pagedRequest.PageIndex, pagedRequest.PageSize, pagedRequest.OrderBy);
            var totalCount = await _serviceService.GetTotalServiceCountAsync(pagedRequest.Keyword,
                Util.TranslateGeneralStatus(pagedRequest.Status));

            var pagedResponse = new PagedResponse<ServiceResponse>(
                services?.Select(s =>
                {
                    var res = _mapper.Map<ServiceResponse>(s);
                    res.Status = Util.TranslateGeneralStatus(s.Status) ?? "Trạng Thái Null";
                    if (s.Promotion != null)
                    {
                        var promotionRes = _mapper.Map<PromotionResponse>(s.Promotion);
                        promotionRes.Status = Util.TranslatePromotionStatus(promotionRes.Status) ?? "Trạng Thái Null";
                        res.Promotion = promotionRes;
                    }
                    return res;
                }) ?? Enumerable.Empty<ServiceResponse>(),
                totalCount,
                pagedRequest.PageIndex,
                pagedRequest.PageSize
            );

            return Ok(new ResponseObject<PagedResponse<ServiceResponse>>("Fetch Service Success", pagedResponse));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceByIdAync(int id)
        {
            try
            {
                var service = await _serviceService.GetServiceByIdAsync(id);
                if (service == null)
                {
                    Ok(new ResponseObject<ServiceResponse>($"Dịch vụ với ID {id} không tìm thấy.", null));
                }
                var response = _mapper.Map<ServiceResponse>(service);
                response.Promotion = _mapper.Map<PromotionResponse>(response.Promotion);
                response.Status = Util.TranslateGeneralStatus(response.Status) ?? "Trạng Thái Null";
                return Ok(new ResponseObject<ServiceResponse>("Fetch Service Success", response));
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
        }

        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedServicesAsync()
        {
            try
            {
                var discountedServices = await _serviceService.GetDiscountedServicesAsync();
                if (discountedServices == null || !discountedServices.Any())
                {
                    return NotFound(new ResponseObject<IEnumerable<ServiceResponse>>("Không tìm thấy dịch vụ nào đang giảm giá."));
                }

                var response = discountedServices.Select(s => _mapper.Map<ServiceResponse>(s));
                return Ok(new ResponseObject<IEnumerable<ServiceResponse>>("Lấy dịch vụ giảm giá thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>("Đã xảy ra lỗi khi lấy dịch vụ.", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceAync([FromBody] ServiceCreateRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Status))
                {
                    throw new ArgumentException("Status không được bỏ trống.", nameof(request.Status));
                }
                request.Status = request.Status.ToUpper();
                await _serviceService.AddServiceAsync(request);

                var serviceResponse = _mapper.Map<ServiceCreateResponse>(_mapper.Map<Service>(request));
                serviceResponse.BranchId = request.BranchId;
                serviceResponse.NewPrice = request.NewPrice;
                return Ok(new ResponseObject<ServiceCreateResponse>("Create Service Success", serviceResponse));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<ServiceCreateResponse>($"Error: {ex.Message}", null));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceCreateResponse>($"Validation Error: {ex.Message}", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceCreateResponse>($"An unexpected error occurred: {ex.Message}", null));
            }
        }

        [HttpPut("{existingServiceId}")]
        public async Task<IActionResult> UpdateServiceAync(int existingServiceId, [FromBody] ServiceUpdateRequest request)
        {
            try
            {

                await _serviceService.UpdateServiceAsync(request, existingServiceId);
                var service = await _serviceService.GetServiceByIdAsync(existingServiceId);
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
        public async Task<IActionResult> DeleteServiceAync(int id)
        {
            try
            {
                await _serviceService.DeleteServiceAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<ServiceResponse>(ex.Message, null));
            }
        }


    }
}
