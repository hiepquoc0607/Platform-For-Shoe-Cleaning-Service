using Microsoft.AspNetCore.Mvc;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IShipService _shipService;
        private readonly HttpClient _httpClient;

        public LocationController(IShipService shipService, IHttpClientFactory httpClientFactory)
        {
            _shipService = shipService;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        [Route("api/locations/provinces")]
        public async Task<IActionResult> GetProvincesAsync()
        {
            var result = await _shipService.GetProvincesAsync(_httpClient);

            if (result == null)
            {
                return NotFound(new
                {
                    status = "error",
                    statusCode = 404,
                    message = "Không Tìm Thấy Dữ Liệu Tỉnh!"
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/locations/{provinceId}/districts")]
        public async Task<IActionResult> GetDistrictsByProvinceIdAsync([FromRoute] int provinceId)
        {
            var result = await _shipService.GetDistrictsByProvinceIdAsync(_httpClient, provinceId);

            if (result == null)
            {
                return NotFound(new
                {
                    status = "error",
                    statusCode = 404,
                    message = "Không Tìm Thấy Dữ Liệu Quận!"
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/locations/{districtId}/wards")]
        public async Task<IActionResult> GetWardsByDistrictIdAsync([FromRoute] int districtId)
        {
            var result = await _shipService.GetWardsByDistrictIdAsync(_httpClient, districtId);

            if (result == null)
            {
                return NotFound(new
                {
                    status = "error",
                    statusCode = 404,
                    message = "Không Tìm Thấy Dữ Liệu Phường!"
                });
            }

            return Ok(result);
        }
    }
}
