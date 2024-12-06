using TP4SCS.Library.Models.Request.Chat;
using TP4SCS.Library.Models.Response.Chat;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IChatService
    {
        Task<ApiResponse<ChatRoomResponse>> CreateChatRoomAsync(ChatRoomRequest roomRequest);

        Task<ApiResponse<IEnumerable<ChatRoomResponse>?>> GetChatsRoomAsync(int accId);

        Task<ApiResponse<MessageResponse>> SendMessageAsync(MessageRequest messageRequest);

        Task<ApiResponse<IEnumerable<MessageResponse>?>> GetMessagesAsync(string roomId);
    }
}
