namespace TP4SCS.Library.Models.Request.Chat
{
    public class ChatRoomRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int AccountId1 { get; set; }

        public int AccountId2 { get; set; }
    }
}
