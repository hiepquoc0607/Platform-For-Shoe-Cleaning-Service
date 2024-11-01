using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        //[Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("api/accounts")]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] GetAccountRequest getAccountRequest)
        {
            var result = await _accountService.GetAccountsAsync(getAccountRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/{id}", Name = "GetAccountById")]
        public async Task<IActionResult> GetAccountByIdAsync([FromRoute] int id)
        {
            var result = await _accountService.GetAccountByIdAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/accounts")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
        {
            var result = await _accountService.CreateAccountAsync(createAccountRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            int newAccId = await _accountService.GetAccountMaxIdAsync();

            return CreatedAtAction("GetAccountById", new { id = newAccId }, result.Data);
        }

        [Authorize]
        [HttpPut]
        [Route("api/accounts/{id}")]
        public async Task<IActionResult> UpdateAccountAsync([FromRoute] int id, UpdateAccountRequest updateAccountRequest)
        {
            string? userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !userIdClaim.Equals(id.ToString()))
            {
                return Forbid();
            }

            var result = await _accountService.UpdateAccountAsync(id, updateAccountRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        [Route("api/accounts/{id}/password")]
        public async Task<IActionResult> UpdateAccountPasswordAsync([FromRoute] int id, UpdateAccountPasswordRequest updateAccountPasswordRequest)
        {
            string? userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !userIdClaim.Equals(id.ToString()))
            {
                return Forbid();
            }

            var result = await _accountService.UpdateAccountPasswordAsync(id, updateAccountPasswordRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        [Route("api/admin/accounts/{id}/status")]
        public async Task<IActionResult> UpdateAccountStatusForAdminAsync([FromRoute] int id, [FromBody] UpdateStatusRequest updateStatusRequest)
        {
            var result = await _accountService.UpdateAccountStatusForAdminAsync(id, updateStatusRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("api/accounts/{id}")]
        public async Task<IActionResult> DeleteAccountAsync([FromRoute] int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
