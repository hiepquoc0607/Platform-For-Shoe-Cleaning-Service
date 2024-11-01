using Microsoft.AspNetCore.Mvc;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Route("api/locations/cities")]
        public async Task<IActionResult> GetCitiesAsync()
        {
            var result = await _locationService.GetCityAsync();

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("api/locations/{cityName}/wards")]
        public async Task<IActionResult> GetWardsByCityAsync([FromRoute] string cityName)
        {
            var result = await _locationService.GetWardByCityAsync(cityName);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("api/locations/{wardName}/provinces")]
        public async Task<IActionResult> GetProvincesByWardAsync([FromRoute] string wardName)
        {
            var result = await _locationService.GetProviceByWardAsync(wardName);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result);
        }
    }
}
