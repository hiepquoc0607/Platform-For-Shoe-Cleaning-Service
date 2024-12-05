using Newtonsoft.Json;
using System.Text;
using TP4SCS.Library.Models.Request.Chat;
using TP4SCS.Library.Models.Response.Chat;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ChatService : IChatService
    {
        private readonly string _firebaseDbUrl = "https://tp4scs-default-rtdb.asia-southeast1.firebasedatabase.app";

        private async Task AddToFirebaseAsync<T>(string path, T data)
        {
            using var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"{_firebaseDbUrl}/{path}.json", content);
        }

        private async Task<T?> GetFromFirebaseAsync<T>(string path)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{_firebaseDbUrl}/{path}.json");
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<ChatRoomResponse?> CreateChatRoomAsync(ChatRoomRequest roomRequest)
        {
            var existingRoom = await GetChatRoomAsync(roomRequest.AccountId1, roomRequest.AccountId2);
            if (existingRoom != null)
            {
                return existingRoom;
            }

            roomRequest.Id = $"{roomRequest.AccountId1}_{roomRequest.AccountId2}";

            await AddToFirebaseAsync($"chatRooms/{roomRequest.Id}", roomRequest);

            var room = new ChatRoomResponse
            {
                Id = roomRequest.Id,
                AccountId1 = roomRequest.AccountId1,
                AccountId2 = roomRequest.AccountId2
            };

            return room;
        }

        public async Task<ChatRoomResponse?> GetChatRoomAsync(int accId1, int accId2)
        {
            return await GetFromFirebaseAsync<ChatRoomResponse>($"chatRooms/{accId1}_{accId2}");
        }

        public async Task<IEnumerable<MessageResponse>> GetMessagesAsync(string roomId)
        {
            var messages = await GetFromFirebaseAsync<Dictionary<string, MessageResponse>>($"messages/{roomId}");

            return messages?.Values.OrderBy(m => m.Timestamp) ?? Enumerable.Empty<MessageResponse>();
        }

        public async Task SendMessageAsync(MessageRequest message)
        {
            await AddToFirebaseAsync($"messages/{message.RoomId}/{message.Id}", message);
        }
    }
}