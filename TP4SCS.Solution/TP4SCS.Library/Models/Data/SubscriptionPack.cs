using System;
using System.Collections.Generic;

namespace TP4SCS.Library.Models.Data;

public partial class SubscriptionPack
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Period { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
