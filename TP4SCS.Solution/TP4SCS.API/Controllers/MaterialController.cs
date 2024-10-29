using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Material;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.MaterialResponse;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IMapper _mapper;

        public MaterialController(IMaterialService materialService, IMapper mapper)
        {
            _materialService = materialService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterials(
            [FromQuery] string? keyword = null,
            [FromQuery] string? status = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            var materials = await _materialService.GetMaterialsAsync(keyword, Util.TranslateGeneralStatus(status),
                pageIndex, pageSize, orderBy);

            var totalCount = await _materialService.GetTotalMaterialCountAsync(keyword, status);

            var pagedResponse = new PagedResponse<MaterialResponse>(
                materials?.Select(m =>
                {
                    var res = _mapper.Map<MaterialResponse>(m);
                    res.Status = Util.TranslateGeneralStatus(m.Status) ?? "Trạng Thái Null";
                    return res;
                }) ?? Enumerable.Empty<MaterialResponse>(),
                totalCount,
                pageIndex,
                pageSize
            );

            return Ok(new ResponseObject<PagedResponse<MaterialResponse>>("Fetch Material Success", pagedResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null)
            {
                return NotFound(new ResponseObject<MaterialResponse>("Material not found.", null));
            }
            return Ok(new ResponseObject<MaterialResponse>("Fetch Material Success", _mapper.Map<MaterialResponse>(material)));
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterial(int serviceId, [FromBody] MaterialCreateRequest materialCreateRequest)
        {
            if (materialCreateRequest == null)
            {
                return BadRequest(new ResponseObject<MaterialResponse>("Yêu cầu tạo Material không được null.", null));
            }

            var material = _mapper.Map<Material>(materialCreateRequest);

            try
            {
                if (string.IsNullOrWhiteSpace(material.Status))
                {
                    throw new ArgumentException("Status không được bỏ trống.", nameof(material.Status));
                }
                material.Status = material.Status.ToUpper();
                await _materialService.AddMaterialAsync(serviceId, material);
                return Ok(new ResponseObject<MaterialResponse>("Create material success", _mapper.Map<MaterialResponse>(material)));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<MaterialResponse>("Lỗi không xác định: " + ex.Message, null));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, [FromBody] MaterialUpdateRequest materialUpdateRequest)
        {
            if (materialUpdateRequest == null)
            {
                return BadRequest(new ResponseObject<MaterialResponse>("Material không được null.", null));
            }

            var material = _mapper.Map<Material>(materialUpdateRequest);
            if (string.IsNullOrWhiteSpace(material.Status))
            {
                throw new ArgumentException("Status không được bỏ trống.", nameof(material.Status));
            }
            material.Status = material.Status.ToUpper();
            try
            {
                await _materialService.UpdateMaterialAsync(id, material);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<MaterialResponse>("Lỗi không xác định: " + ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            try
            {
                await _materialService.DeleteMaterialAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<MaterialResponse>(ex.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<MaterialResponse>("Lỗi không xác định: " + ex.Message, null));
            }
        }
    }
}
