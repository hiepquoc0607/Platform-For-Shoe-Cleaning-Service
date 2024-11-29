using Microsoft.AspNetCore.Mvc;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/leaderboards")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet("/by-month")]
        public async Task<IActionResult> GetLeaderboardByMonthAsync()
        {
            var result = await _leaderboardService.GetLeaderboardByMonthAsync();

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet("/by-year")]
        public async Task<IActionResult> GetLeaderboardByYearAsync()
        {
            var result = await _leaderboardService.GetLeaderboardByMonthAsync();

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLeaderboardAsync()
        {
            var result = await _leaderboardService.UpdateLeaderboardAsync();

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
