using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet]
        [Route("api/businesses")]
        public async Task<IActionResult> GetBusinessProfilesAsync([FromQuery] GetBusinessRequest getBusinessRequest)
        {
            var result = await _businessService.GetBusinessesProfilesAsync(getBusinessRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/businesses/{id}", Name = "GetBusinessProfileById")]
        public async Task<IActionResult> GetBusinessProfileByIdAsync([FromRoute] int id)
        {
            var result = await _businessService.GetBusinessProfileByIdAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/busineses")]
        public async Task<IActionResult> CreateBusinessProfileAsync([FromBody] OwnerRegisterRequest createBusinessRequest)
        {
            string? userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userId = int.TryParse(userIdClaim, out int id);

            var result = await _businessService.CreateBusinessProfileAsync(id, createBusinessRequest);

            if (result.StatusCode != 200 || result.Data == null)
            {
                return StatusCode(result.StatusCode, result);
            }

            return CreatedAtAction("GetBusinessProfileById", new { id = result.Data.Id }, result.Data);
        }

        [HttpPut]
        [Route("api/businesses/{id}")]
        public async Task<IActionResult> UpdateBusinessProfileAsync([FromRoute] int id, [FromBody] UpdateBusinessRequest updateBusinessRequest)
        {
            string? userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userId = int.TryParse(userIdClaim, out int ownerId);

            if (!await _businessService.CheckOwnerOfBusiness(ownerId, id))
            {
                Forbid();
            }

            var result = await _businessService.UpdateBusinessProfileAsync(id, updateBusinessRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        [Route("api/admin/businesses/{id}")]
        public async Task<IActionResult> UpdateBusinessStatusForAdminAsync(int id, [FromBody] UpdateBusinessStatusRequest updateBusinessStatusRequest)
        {
            var result = await _businessService.UpdateBusinessStatusForAdminAsync(id, updateBusinessStatusRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
