namespace TP4SCS.Library.Models.Request.Chat
{
    public class MessageRequest
    {
        public string RoomId { get; set; } = string.Empty;

        public int SenderId { get; set; }

        public string Content { get; set; } = string.Empty;

        public bool IsImage { get; set; }
    }
}
