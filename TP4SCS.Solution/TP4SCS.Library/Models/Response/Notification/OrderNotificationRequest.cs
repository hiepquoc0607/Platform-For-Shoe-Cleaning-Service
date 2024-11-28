namespace TP4SCS.Library.Models.Response.Notification
{
    public class OrderNotificationRequest
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public DateTime NotificationTime { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
