namespace TP4SCS.Library.Models.Data;

public partial class SupportTicket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ModeratorId { get; set; }

    public int CategoryId { get; set; }

    public int? OrderId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual TicketCategory Category { get; set; } = null!;

    public virtual Account Moderator { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual Account User { get; set; } = null!;
}
