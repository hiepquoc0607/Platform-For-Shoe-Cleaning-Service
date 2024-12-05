namespace TP4SCS.Library.Models.Response.Chat
{
    public class MessageResponse
    {
        public string Id { get; set; } = string.Empty;

        public string RoomId { get; set; } = string.Empty;

        public int SenderId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}
