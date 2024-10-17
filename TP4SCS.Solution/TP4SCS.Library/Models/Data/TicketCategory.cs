using System;
using System.Collections.Generic;

namespace TP4SCS.Library.Models.Data;

public partial class TicketCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
}
