using TP4SCS.Library.Models.Request.Chat;
using TP4SCS.Library.Models.Response.Chat;

namespace TP4SCS.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatRoomResponse?> CreateChatRoomAsync(ChatRoomRequest roomRequest);

        Task<ChatRoomResponse?> GetChatRoomAsync(int accId1, int accId2);

        Task SendMessageAsync(MessageRequest message);

        Task<IEnumerable<MessageResponse>> GetMessagesAsync(string roomId);
    }
}
