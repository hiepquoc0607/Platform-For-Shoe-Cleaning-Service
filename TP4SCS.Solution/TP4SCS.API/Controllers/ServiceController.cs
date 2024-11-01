using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.AssetUrl;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Promotion;
using TP4SCS.Library.Models.Response.Service;
using TP4SCS.Library.Utils.Utils;
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
                pagedRequest.Status,
                pagedRequest.PageIndex, pagedRequest.PageSize, pagedRequest.OrderBy);
            var totalCount = await _serviceService.GetTotalServiceCountAsync(pagedRequest.Keyword,
                pagedRequest.Status);

            var pagedResponse = new PagedResponse<ServiceResponse>(
                services?.Select(s =>
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
                        res.AssetUrl = assetRes;
                    }
                    return res;
                }) ?? Enumerable.Empty<ServiceResponse>(),
                totalCount,
                pagedRequest.PageIndex,
                pagedRequest.PageSize
            );

            return Ok(new ResponseObject<PagedResponse<ServiceResponse>>("Fetch Service Success", pagedResponse));
        }
        [HttpGet("branches/{id}")]
        public async Task<IActionResult> GetServicesByBranchIdAync([FromRoute] int id)
        {
            var services = await _serviceService.GetServicesByBranchIdAsync(id);
            return Ok(new ResponseObject<IEnumerable<ServiceResponse>>("Fetch Service By Branch Id Success",
                services.Adapt<IEnumerable<ServiceResponse>>()));
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
        [HttpGet("grouped")]
        public async Task<IActionResult> GetServicesGroupByNameAsync(
            [FromQuery] int? pageIndex = null,
            [FromQuery] int? pageSize = null)
        {
            if (pageIndex.HasValue && pageIndex.Value < 1)
            {
                return BadRequest(new ResponseObject<string>("Chỉ số trang phải lớn hơn 0."));
            }
            if (pageSize.HasValue && pageSize.Value < 1)
            {
                return BadRequest(new ResponseObject<string>("Kích thước trang phải lớn hơn 0."));
            }

            var (services, totalCount) = await _serviceService.GetServicesGroupByNameAsync(pageIndex, pageSize, OrderByEnum.IdAsc);

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                var pageResponse = new PagedResponse<ServiceResponse>(
                        services.Select(s => _mapper.Map<ServiceResponse>(s)),
                        totalCount,
                        pageIndex.Value,
                        pageSize.Value);

                return Ok(new ResponseObject<PagedResponse<ServiceResponse>>("Fetch Service Grouped Success", pageResponse));
            }

            return Ok(new ResponseObject<IEnumerable<ServiceResponse>>("Fetch Service Grouped Success",
                    services.Select(s => _mapper.Map<ServiceResponse>(s))));
        }



        [HttpGet("by-name-and-branch")]
        public async Task<IActionResult> GetServiceByNameAndBranchIdAsync(
            [FromQuery] string name, [FromQuery] int branchId)
        {
            try
            {
                var service = await _serviceService.GetServicesByNameAndBranchIdAsync(name, branchId);

                if (service == null)
                {
                    return NotFound(new ResponseObject<ServiceResponse>($"Không tìm thấy dịch vụ với tên '{name}' và branchId '{branchId}'", null));
                }

                var response = _mapper.Map<ServiceResponse>(service);
                return Ok(new ResponseObject<ServiceResponse>("Lấy dịch vụ thành công", response));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ResponseObject<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>("Đã xảy ra lỗi khi lấy dịch vụ", ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateServiceAync([FromForm] ServiceCreateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Status) || !Util.IsValidGeneralStatus(request.Status))
                {
                    throw new ArgumentException("Status của Service không hợp lệ.", nameof(request.Status));
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceStatusAync(int id, [FromBody] string status)
        {
            try
            {

                await _serviceService.UpdateServiceStatusAsync(status, id);
                var service = await _serviceService.GetServiceByIdAsync(id);
                return Ok(new ResponseObject<ServiceResponse>("Update Status Service with Id: {id} Success",
                    _mapper.Map<ServiceResponse>(service)));
            }

            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceResponse>(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new ResponseObject<ServiceResponse>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceResponse>($"An error occurred: {ex.Message}"));
            }
        }
        [HttpPut("bulk-update")]
        public async Task<IActionResult> BulkUpdateServiceAsync(
            [FromBody] ServiceUpdateRequest serviceUpdateRequest,
            [FromQuery] ExistingServiceRequest existingServiceRequest)
        {
            if (serviceUpdateRequest == null)
            {
                return BadRequest("Yêu cầu cập nhật dịch vụ không được để trống.");
            }

            try
            {
                await _serviceService.UpdateServiceAsync(serviceUpdateRequest, existingServiceRequest);
                return Ok("Các dịch vụ đã được cập nhật thành công.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
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
