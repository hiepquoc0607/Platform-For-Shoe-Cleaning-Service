using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("api/accounts")]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] GetAccountRequest getAccountRequest)
        {
            var result = await _accountService.GetAccountsAsync(getAccountRequest);

            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/accounts/{id}", Name = "GetAccountById")]
        public async Task<IActionResult> GetAccountByIdAsync([FromRoute] int id)
        {
            var result = await _accountService.GetAccountByIdAsync(id);

            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/accounts")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
        {
            var result = await _accountService.CreateAccountAsync(createAccountRequest);

            if (!ModelState.IsValid)
            {
                return BadRequest("Trường Nhập Không Hợp Lệ Hoặc Thiếu!");
            }

            if (!result.Status.Equals("error"))
            {
                return BadRequest(result);
            }

            int newAccId = await _accountService.GetAccountMaxIdAsync();

            return CreatedAtAction("GetAccountById", new { id = newAccId }, result.Data);
        }

        [HttpPut]
        [Route("api/accounts/{id}")]
        public async Task<IActionResult> UpdateAccountAsync([FromRoute] int id, UpdateAccountRequest updateAccountRequest)
        {
            var result = await _accountService.UpdateAccountAsync(id, updateAccountRequest);

            if (!result.Status.Equals("error"))
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("api/admin/accounts/{id}/status")]
        public async Task<IActionResult> UpdateAccountStatusForAdminAsync([FromRoute] int id, [FromBody] string status)
        {
            var result = await _accountService.UpdateAccountStatusForAdminAsync(id, status);

            if (!result.Status.Equals("error"))
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/accounts/{id}")]
        public async Task<IActionResult> DeleteAccountAsync([FromRoute] int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);

            if (!result.Status.Equals("error"))
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
