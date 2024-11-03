using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.AssetUrl;
using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.BranchService;
using TP4SCS.Library.Models.Response.Category;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Promotion;
using TP4SCS.Library.Models.Response.Service;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Services.Interfaces;

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
            var services = await _serviceService.GetServicesAsync(
                pagedRequest.Keyword,
                pagedRequest.Status,
                pagedRequest.PageIndex,
                pagedRequest.PageSize,
                pagedRequest.OrderBy
            );

            var totalCount = await _serviceService.GetTotalServiceCountAsync(
                pagedRequest.Keyword,
                pagedRequest.Status
            );

            var pagedResponse = new PagedResponse<ServiceResponse>(
                services?.Select(s =>
                {
                    // Ánh xạ Service sang ServiceResponse
                    var res = _mapper.Map<ServiceResponse>(s);
                    res.Status = Util.TranslateGeneralStatus(s.Status);

                    // Ánh xạ Promotion nếu có
                    if (s.Promotion != null)
                    {
                        var promotionRes = _mapper.Map<PromotionResponse>(s.Promotion);
                        promotionRes.Status = Util.TranslateGeneralStatus(promotionRes.Status);
                        res.Promotion = promotionRes;
                    }

                    // Ánh xạ AssetUrls nếu có
                    if (s.AssetUrls != null && s.AssetUrls.Any())
                    {
                        var assetRes = _mapper.Map<List<AssetUrlResponse>>(s.AssetUrls);
                        res.AssetUrls = assetRes;
                    }

                    // Ánh xạ BranchServices và các Branch bên trong
                    if (s.BranchServices != null && s.BranchServices.Any())
                    {
                        var branchServiceResponses = s.BranchServices.Select(bs =>
                        {
                            var branchServiceResponse = _mapper.Map<BranchServiceResponse>(bs);

                            // Ánh xạ Branch trong BranchServiceResponse
                            branchServiceResponse.Branch = _mapper.Map<BranchResponse>(bs.Branch);

                            return branchServiceResponse;
                        }).ToList();

                        res.BranchServices = branchServiceResponses;
                    }

                    return res;
                }) ?? Enumerable.Empty<ServiceResponse>(),
                totalCount,
                pagedRequest.PageIndex,
                pagedRequest.PageSize
            );

            return Ok(new ResponseObject<PagedResponse<ServiceResponse>>("Lấy dịch vụ thành công", pagedResponse));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceByIdAync(int id)
        {
            try
            {
                var service = await _serviceService.GetServiceByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new ResponseObject<ServiceResponse>($"Dịch vụ với ID {id} không tìm thấy.", null));
                }

                // Ánh xạ Service sang ServiceResponse
                var response = _mapper.Map<ServiceResponse>(service);

                // Ánh xạ Category
                response.Category = _mapper.Map<ServiceCategoryResponse>(service.Category);

                // Ánh xạ Promotion
                if (service.Promotion != null)
                {
                    var promotionRes = _mapper.Map<PromotionResponse>(service.Promotion);
                    promotionRes.Status = Util.TranslateGeneralStatus(promotionRes.Status);
                    response.Promotion = promotionRes;
                }

                // Ánh xạ AssetUrls
                if (response.AssetUrls != null && response.AssetUrls.Any())
                {
                    var assetRes = _mapper.Map<List<AssetUrlResponse>>(response.AssetUrls);
                    response.AssetUrls = assetRes;
                }

                // Ánh xạ BranchServices và Branch bên trong
                if (service.BranchServices != null && service.BranchServices.Any())
                {
                    var branchServiceResponses = service.BranchServices.Select(bs =>
                    {
                        var branchServiceResponse = _mapper.Map<BranchServiceResponse>(bs);

                        // Ánh xạ Branch trong BranchServiceResponse
                        branchServiceResponse.Branch = _mapper.Map<BranchResponse>(bs.Branch);

                        return branchServiceResponse;
                    }).ToList();

                    response.BranchServices = branchServiceResponses;
                }

                // Dịch trạng thái
                response.Status = Util.TranslateGeneralStatus(response.Status) ?? "Trạng thái null";

                return Ok(new ResponseObject<ServiceResponse>("Lấy dịch vụ thành công", response));
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
        public async Task<IActionResult> CreateServiceAsync([FromBody] ServiceCreateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Status) || !Util.IsValidGeneralStatus(request.Status))
                {
                    throw new ArgumentException("Trạng thái của dịch vụ không hợp lệ.", nameof(request.Status));
                }

                // Đặt trạng thái lên chữ hoa
                request.Status = request.Status.ToUpper();

                if (!HttpContext.Request.Headers.TryGetValue("X-Business-ID", out var businessIdValue) || !int.TryParse(businessIdValue, out var businessId))
                {
                    return BadRequest(new ResponseObject<ServiceCreateResponse>("Business ID không hợp lệ hoặc không tìm thấy trong header."));
                }

                await _serviceService.AddServiceAsync(request, businessId);
                return Ok(new ResponseObject<string>("Tạo dịch vụ thành công"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<ServiceCreateResponse>($"Lỗi: {ex.Message}", null));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceCreateResponse>($"Lỗi xác thực: {ex.Message}", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceCreateResponse>($"Đã xảy ra lỗi không mong muốn: {ex.Message}", null));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceAsync([FromBody] ServiceUpdateRequest serviceUpdateRequest, int id)
        {
            if (serviceUpdateRequest == null)
            {
                return BadRequest("Yêu cầu cập nhật dịch vụ không được để trống.");
            }

            try
            {
                await _serviceService.UpdateServiceAsync(serviceUpdateRequest, id);
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

        [HttpGet("business")]
        public async Task<IActionResult> GetServiceByBusinessId([FromQuery] GetBusinessServiceRequest getBusinessServiceRequest)
        {
            var result = await _serviceService.GetServiceByBusinessIdAsync(getBusinessServiceRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
