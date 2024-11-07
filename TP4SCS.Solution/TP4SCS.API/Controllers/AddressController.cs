using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Address;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Route("api/addresses/account/{id}")]
        public async Task<IActionResult> GetAddressesByAccountIdAsync([FromRoute] int id)
        {
            var result = await _addressService.GetAddressesByAccountIdAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/addresses/{id}")]
        public async Task<IActionResult> GetAddressesByIdAsync([FromRoute] int id)
        {
            var result = await _addressService.GetAddressesByIdAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/addresses")]
        public async Task<IActionResult> CreateAddressAsync([FromBody] CreateAddressRequest createAddressRequest)
        {
            var result = await _addressService.CreateAddressAsync(createAddressRequest);

            if (!ModelState.IsValid)
            {
                return BadRequest("Trường Nhập Không Hợp Lệ Hoặc Thiếu!");
            }

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            int newAddId = await _addressService.GetAddressMaxIdAsync();

            return StatusCode(201, result.Data);
        }

        [HttpPut]
        [Route("api/addresses/{id}")]
        public async Task<IActionResult> UpdateAddressAsync([FromRoute] int id, [FromBody] UpdateAddressRequest updateAddressRequest)
        {
            var result = await _addressService.UpdateAddressAsync(id, updateAddressRequest);

            if (!ModelState.IsValid)
            {
                return BadRequest("Trường Nhập Không Hợp Lệ Hoặc Thiếu!");
            }

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("api/addresses/{id}/default")]
        public async Task<IActionResult> UpdateAddressDefaultAsync([FromRoute] int id)
        {
            var result = await _addressService.UpdateAddressDefaultAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("api/addresses/{id}")]
        public async Task<IActionResult> DeteleAddresAsync([FromRoute] int id)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
