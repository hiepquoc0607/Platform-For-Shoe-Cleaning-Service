using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetUrlController : ControllerBase
    {
        private readonly IAssetUrlService _assetUrlService;
        public AssetUrlController(IAssetUrlService assetUrlService)
        {
            _assetUrlService = assetUrlService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected");

            var result = await _assetUrlService.UploadFileAsync(file);
            return Ok(new { Url = result });
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveImage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL cannot be null or empty.");
            }

            await _assetUrlService.DeleteImageAsync(url);
            return Ok();
        }
    }
}
