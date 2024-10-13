using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] GetAccountRequest getAccountRequest)
        {
            var result = await _accountService.GetAccountsAsync(getAccountRequest);

            if (result == null)
            {
                return NotFound("No Accounts Been Found!");
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountByIdAsync([FromRoute] int id)
        {
            var result = await _accountService.GetAccountByIdAsync(id);

            if (result == null)
            {
                return NotFound("No Accounts Been Found!");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest, [FromQuery] RoleRequest roleRequest)
        {
            var result = await _accountService.CreateAccountAsync(createAccountRequest, (int)roleRequest);

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Or Missing Input Fields!");
            }

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            int newAccId = await _accountService.GetAccountMaxIdAsync();
            var newAcc = await _accountService.GetAccountByIdAsync(newAccId);

            return CreatedAtAction(nameof(GetAccountByIdAsync), new { id = newAccId }, newAcc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountAsync([FromRoute] int id, UpdateAccountRequest updateAccountRequest)
        {
            var result = await _accountService.UpdateAccountAsync(id, updateAccountRequest);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok("Account Updated Successfully!");
        }

        [HttpPut("admin/{id}/status")]
        public async Task<IActionResult> UpdateAccountStatusForAdminAsync([FromRoute] int id, StatusAdminRequest status)
        {
            var result = await _accountService.UpdateAccountStatusForAdminAsync(id, status);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok("Account Unsuspended Successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountAsync([FromRoute] int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok("Account Deleted Successfully!");
        }
    }
}
