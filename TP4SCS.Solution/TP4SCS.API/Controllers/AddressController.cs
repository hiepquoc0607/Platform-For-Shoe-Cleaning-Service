using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Address;
using TP4SCS.Services.Implements;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
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

            if (result == null)
            {
                return NotFound("Địa chỉ không tồn tại!");
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/addresses/{id}", Name = "GetAddressById")]
        public async Task<IActionResult> GetAddressesByIdAsync([FromRoute] int id)
        {
            var result = await _addressService.GetAddressesByIdAsync(id);

            if (result == null)
            {
                return NotFound("Địa chỉ không tồn tại!");
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

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            int newAddId = await _addressService.GetAddressMaxIdAsync();
            var newAdd = await _addressService.GetAddressesByIdAsync(newAddId);

            return CreatedAtAction("GetAddressById", new { id = newAddId }, newAdd);
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

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPut]
        [Route("api/addresses/{id}/default")]
        public async Task<IActionResult> UpdateAddressDefaultAsync([FromRoute] int id)
        {
            var result = await _addressService.UpdateAddressDefaultAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpDelete]
        [Route("api/addresses/{id}")]
        public async Task<IActionResult> DeteleAddresAsync([FromRoute] int id)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
