using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Material;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Material;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;

        public MaterialController(IMaterialService materialService, IMapper mapper, IBusinessService businessService)
        {
            _materialService = materialService;
            _mapper = mapper;
            _businessService = businessService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseObject<PagedResponse<MaterialResponse>>>> GetMaterialsAsync(
                    [FromQuery] string? keyword = null,
                    [FromQuery] string? status = null,
                    [FromQuery] OrderByEnum orderBy = OrderByEnum.IdDesc,
                    [FromQuery] int pageIndex = 1,
                    [FromQuery] int pageSize = 10)
        {
            try
            {
                var (materials, total) = await _materialService.GetMaterialsAsync(keyword, status, orderBy, pageIndex, pageSize);

                if (materials == null || !materials.Any())
                {
                    var emptyResponse = new PagedResponse<MaterialResponse>(new List<MaterialResponse>(), 0, pageIndex, pageSize);
                    var response = new ResponseObject<PagedResponse<MaterialResponse>>("No materials found.", emptyResponse);
                    return Ok(response);
                }

                // Chuyển đổi dữ liệu từ Material sang MaterialResponse
                var materialResponses = _mapper.Map<IEnumerable<MaterialResponse>>(materials);

                var pagedResponse = new PagedResponse<MaterialResponse>(materialResponses, total, pageIndex, pageSize);

                var successResponse = new ResponseObject<PagedResponse<MaterialResponse>>("Materials retrieved successfully.", pagedResponse);

                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<PagedResponse<MaterialResponse>>($"Error: {ex.Message}", null);
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet("branches/{branchId}")]
        public async Task<ActionResult<ResponseObject<PagedResponse<MaterialResponse>>>> GetMaterialsByBranchIdAsync(
            int branchId,
            [FromQuery] string? keyword = null,
            [FromQuery] string? status = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            try
            {
                // Lấy dữ liệu từ service
                var (materials, total) = await _materialService.GetMaterialsByBranchIdAsync(branchId, keyword, status, pageIndex, pageSize, orderBy);

                if (materials == null || !materials.Any())
                {
                    var emptyResponse = new PagedResponse<MaterialResponse>(new List<MaterialResponse>(), 0, pageIndex, pageSize);
                    var response = new ResponseObject<PagedResponse<MaterialResponse>>("No materials found for this branch.", emptyResponse);
                    return Ok(response);
                }

                var materialResponses = _mapper.Map<IEnumerable<MaterialResponse>>(materials);

                var pagedResponse = new PagedResponse<MaterialResponse>(materialResponses, total, pageIndex, pageSize);

                var successResponse = new ResponseObject<PagedResponse<MaterialResponse>>("Materials retrieved successfully.", pagedResponse);

                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<PagedResponse<MaterialResponse>>($"Error: {ex.Message}", null);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("businesses/{businessId}")]
        public async Task<ActionResult<ResponseObject<PagedResponse<MaterialResponse>>>> GetMaterialsByBusinessIdAsync(
            int businessId,
            [FromQuery] string? keyword = null,
            [FromQuery] string? status = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            try
            {
                // Lấy dữ liệu từ service
                var (materials, total) = await _materialService.GetMaterialsByBusinessIdAsync(businessId, keyword, status, pageIndex, pageSize, orderBy);

                if (materials == null || !materials.Any())
                {
                    var emptyResponse = new PagedResponse<MaterialResponse>(new List<MaterialResponse>(), 0, pageIndex, pageSize);
                    var response = new ResponseObject<PagedResponse<MaterialResponse>>("No materials found for this business.", emptyResponse);
                    return Ok(response);
                }

                // Chuyển đổi dữ liệu từ Material sang MaterialResponse
                var materialResponses = _mapper.Map<IEnumerable<MaterialResponse>>(materials);

                var pagedResponse = new PagedResponse<MaterialResponse>(materialResponses, total, pageIndex, pageSize);
                var successResponse = new ResponseObject<PagedResponse<MaterialResponse>>("Materials retrieved successfully.", pagedResponse);

                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<PagedResponse<MaterialResponse>>($"Error: {ex.Message}", null);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseObject<MaterialResponse>>> AddMaterialAsync(
            [FromBody] MaterialCreateRequest materialRequest)
        {
            try
            {
                if (materialRequest == null)
                {
                    return BadRequest(new ResponseObject<MaterialResponse>("Yêu cầu thêm nguyên liệu không được để trống.", null));
                }

                if (materialRequest.Price <= 0)
                {
                    return BadRequest(new ResponseObject<MaterialResponse>("Giá phải lớn hơn 0.", null));
                }

                if (materialRequest.AssetUrls == null || !materialRequest.AssetUrls.Any())
                {
                    return BadRequest(new ResponseObject<MaterialResponse>("Hình ảnh không được để trống.", null));
                }

                string? userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdClaim, out int id);

                var businessId = await _businessService.GetBusinessIdByOwnerIdAsync(id);

                if (businessId == null)
                {
                    throw new ArgumentException("Account này chưa có doanh nghiệp nào.");
                }
                await _materialService.AddMaterialAsync(materialRequest, businessId.Value);

                // Chuyển đổi dữ liệu từ Material sang MaterialResponse
                var materialResponse = _mapper.Map<MaterialResponse>(materialRequest);

                var successResponse = new ResponseObject<MaterialResponse>("Thêm nguyên liệu thành công.", materialResponse);

                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<MaterialResponse>($"Lỗi: {ex.Message}", null);
                return StatusCode(500, errorResponse);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterialAsync([FromBody] MaterialUpdateRequest materialUpdateRequest, int id)
        {
            if (materialUpdateRequest == null)
            {
                return BadRequest("Yêu cầu cập nhật nguyên liệu không được để trống.");
            }

            try
            {
                // Gọi service để cập nhật nguyên liệu
                await _materialService.UpdateMaterialAsync(materialUpdateRequest, id);
                return Ok("Nguyên liệu đã được cập nhật thành công.");
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
        public async Task<IActionResult> DeleteMaterialAsync(int id)
        {
            try
            {
                // Gọi service để xóa nguyên liệu
                await _materialService.DeleteMaterialAsync(id);
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (KeyNotFoundException ex)
            {
                // Trường hợp không tìm thấy nguyên liệu
                return NotFound(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (Exception ex)
            {
                // Trường hợp có lỗi khác
                return StatusCode(500, new ResponseObject<string>("Lỗi máy chủ nội bộ", ex.Message));
            }
        }

    }
}
