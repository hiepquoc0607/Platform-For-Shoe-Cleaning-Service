namespace TP4SCS.Library.Models.Response.Ticket
{
    public class TicketsResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public int? ModeratorId { get; set; }

        public string? ModeratorName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int Priority { get; set; }

        public int? OrderId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
