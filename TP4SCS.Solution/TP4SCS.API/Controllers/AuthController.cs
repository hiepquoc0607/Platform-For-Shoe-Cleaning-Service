using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            var result = await _accountService.LoginAsync(loginRequest);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok(result.Token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest, [FromQuery] RegisterRoleRequest roleRequest)
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

            return Ok("Register Successfully!");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _accountService.ResetPasswordAsync(resetPasswordRequest);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok("Reset Password Successfully!");
        }
    }
}
