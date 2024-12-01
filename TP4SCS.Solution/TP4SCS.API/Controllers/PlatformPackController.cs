using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.SubscriptionPack;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/platform-packs")]
    [ApiController]
    public class PlatformPackController : ControllerBase
    {
        private readonly IPlatformPackService _subscriptionService;

        public PlatformPackController(IPlatformPackService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPacksAsync()
        {
            var result = await _subscriptionService.GetPacksAsync();

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackByIdAsync([FromRoute] int id)
        {
            var result = await _subscriptionService.GetPackByIdAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackAsync([FromBody] PlatformPackRequest subscriptionPackRequest)
        {
            var result = await _subscriptionService.CreatePackAsync(subscriptionPackRequest);

            if (result.StatusCode != 201)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackAsync([FromRoute] int id, [FromBody] PlatformPackRequest subscriptionPackRequest)
        {
            var result = await _subscriptionService.UpdatePackAsync(id, subscriptionPackRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackAsync([FromRoute] int id)
        {
            var result = await _subscriptionService.DeletePackAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
