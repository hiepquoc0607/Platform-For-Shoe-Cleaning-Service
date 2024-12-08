using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using TP4SCS.Library.Models.Request.Chat;
using TP4SCS.Library.Models.Response.Chat;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class ChatService : IChatService
    {
        private readonly string _firebaseDbUrl = "https://tp4scs-default-rtdb.asia-southeast1.firebasedatabase.app";
        private readonly IAccountRepository _accountRepository;

        public ChatService(IConfiguration configuration, IAccountRepository accountRepository)
        {
            var firebaseFilePath = configuration["Firebase:FilePath"];

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseFilePath)
                });
            }

            _accountRepository = accountRepository;
        }

        private async Task AddToFirebaseAsync<T>(string path, T data)
        {
            using var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PutAsync($"{_firebaseDbUrl}/{path}.json", content);
        }

        //private async Task UpdateToFirebaseAsync<T>(string path, T data)
        //{
        //    using var httpClient = new HttpClient();

        //    var json = JsonConvert.SerializeObject(data);

        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    await httpClient.PutAsync($"{_firebaseDbUrl}/{path}.json", content);
        //}

        private async Task<T?> GetFromFirebaseAsync<T>(string path)
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync($"{_firebaseDbUrl}/{path}.json");

            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<ApiResponse<ChatRoomResponse>> CreateChatRoomAsync(ChatRoomRequest roomRequest)
        {
            var existingRoom1 = await GetChatRoomAsync(roomRequest.AccountId1, roomRequest.AccountId2);

            var existingRoom2 = await GetChatRoomAsync(roomRequest.AccountId1, roomRequest.AccountId2);

            if (existingRoom1 != null || existingRoom2 != null)
            {
                return new ApiResponse<ChatRoomResponse>("error", "Phòng Chat Đã Tồn Tại!", existingRoom1, 400);
            }

            var room = new ChatRoomResponse
            {
                Id = $"{roomRequest.AccountId1}_{roomRequest.AccountId2}",
                AccountId1 = roomRequest.AccountId1,
                IsAccount1Seen = true,
                AccountId2 = roomRequest.AccountId2,
                IsAccount2Seen = true
            };

            try
            {
                await AddToFirebaseAsync($"chatRooms/{room.Id}", room);

                return new ApiResponse<ChatRoomResponse>("success", "Tạo Phòng Chat Thành Công!", room);
            }
            catch (Exception)
            {
                return new ApiResponse<ChatRoomResponse>("error", 400, "Tạo Phòng Chat Thất Bại!");
            }

        }

        private async Task<ChatRoomResponse?> GetChatRoomAsync(int accId1, int accId2)
        {
            return await GetFromFirebaseAsync<ChatRoomResponse>($"chatRooms/{accId1}_{accId2}");
        }

        public async Task<ApiResponse<IEnumerable<ChatRoomResponse>?>> GetChatRoomsAsync(int accId)
        {
            try
            {
                var chatRooms = await GetFromFirebaseAsync<Dictionary<string, ChatRoomResponse>>("chatRooms");

                if (chatRooms == null)
                {
                    return new ApiResponse<IEnumerable<ChatRoomResponse>?>("error", 404, "Thông Tin Phòng Chat Trống!");
                };

                var filteredRooms = chatRooms
                    .Where(cr => cr.Key.Split('_').Contains(accId.ToString()))
                    .Select(cr => cr.Value)
                    .Distinct()
                    .ToList();

                if (filteredRooms == null)
                {
                    return new ApiResponse<IEnumerable<ChatRoomResponse>?>("error", 404, "Không Tìm Thấy Thông Tin Phòng Chat!");
                }

                return new ApiResponse<IEnumerable<ChatRoomResponse>?>("success", "Lấy Thông Tin Phòng Chat Thành Công!", filteredRooms);
            }
            catch (Exception)
            {
                return new ApiResponse<IEnumerable<ChatRoomResponse>?>("error", 400, "Lấy Thông Tin Phòng Chat Thất Bại!");
            }
        }

        public async Task<ApiResponse<IEnumerable<MessageResponse>?>> GetMessagesAsync(int id, string roomId)
        {
            try
            {
                var room = await GetFromFirebaseAsync<ChatRoomResponse>($"chatRooms/{roomId}");

                if (room == null)
                {
                    return new ApiResponse<IEnumerable<MessageResponse>?>("error", 404, "Không Tìm Thấy Thông Tin Chat!");
                }

                if (room.AccountId1 == id)
                {
                    room.IsAccount1Seen = true;
                }
                else
                {
                    room.IsAccount2Seen = true;
                }

                await AddToFirebaseAsync($"chatRooms/{roomId}", room);

                var messages = await GetFromFirebaseAsync<Dictionary<string, MessageResponse>>($"messages/{roomId}");

                if (messages == null)
                {
                    return new ApiResponse<IEnumerable<MessageResponse>?>("error", 404, "Không Tìm Thấy Thông Tin Chat!");
                }

                var result = messages?.Values.OrderBy(m => m.Timestamp) ?? Enumerable.Empty<MessageResponse>();

                return new ApiResponse<IEnumerable<MessageResponse>?>("success", "Lấy Thông Tin Chat Thành Công!", result);
            }
            catch (Exception)
            {
                return new ApiResponse<IEnumerable<MessageResponse>?>("error", 400, "Lấy Thông Tin Chat Thất Bại!");
            }
        }

        public async Task<ApiResponse<MessageResponse>> SendMessageAsync(int id, MessageRequest messageRequest)
        {
            var room = await GetFromFirebaseAsync<ChatRoomResponse>($"chatRooms/{messageRequest.RoomId}");

            if (room == null)
            {
                return new ApiResponse<MessageResponse>("error", 404, "Không Tìm Thấy Thông Tin Chat!");
            }

            if (room.AccountId1 == id)
            {
                room.IsAccount1Seen = true;
                room.IsAccount2Seen = false;
            }
            else
            {
                room.IsAccount1Seen = false;
                room.IsAccount2Seen = true;
            }

            if (string.IsNullOrEmpty(messageRequest.Content) && messageRequest.IsImage == true)
            {
                return new ApiResponse<MessageResponse>("error", 400, "Message Đang Là Nội Dung Ảnh!");
            }


            if (messageRequest.ImageUrls!.Count > 0 && messageRequest.IsImage == false)
            {
                return new ApiResponse<MessageResponse>("error", 400, "Message Đang Là Nội Dung Chữ!");
            }

            var account = await _accountRepository.GetAccountByIdNoTrackingAsync(messageRequest.SenderId);

            if (account == null)
            {
                return new ApiResponse<MessageResponse>("error", 404, "Không Tìm Thấy Thông Tin Tài Khoản");
            }

            var message = new MessageResponse
            {
                Id = Guid.NewGuid().ToString(),
                RoomId = messageRequest.RoomId,
                SenderId = account.Id,
                SenderFullName = account.FullName,
                SenderImageUrl = account.ImageUrl!,
                IsImage = messageRequest.IsImage,
                Timestamp = DateTime.Now,
            };

            if (messageRequest.IsImage == false)
            {
                message.Content = messageRequest.Content;
                message.ImageUrls = null;
            }
            else
            {
                message.Content = null;
                message.ImageUrls = message.ImageUrls;
            }

            try
            {
                await AddToFirebaseAsync($"chatRooms/{messageRequest.RoomId}", room);

                await AddToFirebaseAsync($"messages/{message.RoomId}/{message.Id}", message);

                return new ApiResponse<MessageResponse>("success", "Gửi Tin Nhắn Thành Công!", message, 201);
            }
            catch (Exception)
            {
                return new ApiResponse<MessageResponse>("error", 400, "Gửi Tin Nhắn Thất Bại!");
            }
        }
    }
}