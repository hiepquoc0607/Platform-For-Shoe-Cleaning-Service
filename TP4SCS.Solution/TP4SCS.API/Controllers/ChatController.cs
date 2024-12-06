using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Chat;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("create-room")]
        public async Task<IActionResult> CreateChatRoom([FromBody] ChatRoomRequest room)
        {
            var result = await _chatService.CreateChatRoomAsync(room);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
        {
            var result = await _chatService.SendMessageAsync(messageRequest);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-messages/{roomId}")]
        public async Task<IActionResult> GetMessages([FromRoute] string roomId)
        {
            var result = await _chatService.GetMessagesAsync(roomId);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpGet("get-rooms/{accId}")]
        public async Task<IActionResult> GetChatsRoom([FromRoute] int accId)
        {
            var result = await _chatService.GetChatsRoomAsync(accId);

            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
