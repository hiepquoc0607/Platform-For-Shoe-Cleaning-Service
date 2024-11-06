using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public AuthController(IAuthService authService, IAccountService accountService)
        {
            _authService = authService;
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshToken refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("customer-register")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
        {
            var result = await _authService.CustomerRegisterAsync(createAccountRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("send-verification-email")]
        public async Task<IActionResult> SendVerificationEmailAsync([FromQuery] EmailRequest emailRequest)
        {
            var result = await _authService.SendVerificationEmailAsync(emailRequest.Email);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] VerifyEmailRequest verifyEmailRequest)
        {
            var result = await _authService.VerifyEmailAsync(verifyEmailRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("request-reset-password")]
        public async Task<IActionResult> RequestResetPasswordAsync([FromBody] EmailRequest emailRequest)
        {
            var result = await _authService.RequestResetPasswordAsync(emailRequest.Email);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromQuery] ResetPasswordQuery resetPasswordQuery, [FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordQuery, resetPasswordRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
