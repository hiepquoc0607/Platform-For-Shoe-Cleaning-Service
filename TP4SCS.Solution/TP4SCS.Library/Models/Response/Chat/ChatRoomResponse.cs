namespace TP4SCS.Library.Models.Response.Chat
{
    public class ChatRoomResponse
    {
        public string Id { get; set; } = string.Empty;

        public int AccountId1 { get; set; }

        public bool IsAccount1Seen { get; set; }

        public int AccountId2 { get; set; }

        public bool IsAccount2Seen { get; set; }
    }
}
