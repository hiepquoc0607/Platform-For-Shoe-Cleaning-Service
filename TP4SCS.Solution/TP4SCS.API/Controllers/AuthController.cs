using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

        //[HttpPost("register")]
        //public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
        //{
        //    var result = await _authService.CreateAccountAsync(createAccountRequest);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Trường Nhập Không Hợp Lệ Hoặc Thiếu!");
        //    }

        //    if (!result.IsSuccess)
        //    {
        //        return BadRequest(result.Message);
        //    }

        //    return Ok(result.Message);
        //}

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordRequest);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
