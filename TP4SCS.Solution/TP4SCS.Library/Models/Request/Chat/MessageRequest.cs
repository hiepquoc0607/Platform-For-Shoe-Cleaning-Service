namespace TP4SCS.Library.Models.Request.Chat
{
    public class MessageRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RoomId { get; set; } = string.Empty;

        public int SenderId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
