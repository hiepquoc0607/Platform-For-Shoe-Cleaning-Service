using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var result = await _accountService.LoginAsync(loginRequest);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok(result.Token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
        {
            var result = await _accountService.CreateAccountAsync(createAccountRequest);

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

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _accountService.ResetPasswordAsync(resetPasswordRequest);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok(result.Message);
        }
    }
}
