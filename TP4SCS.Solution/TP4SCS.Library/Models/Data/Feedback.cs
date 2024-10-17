using System;
using System.Collections.Generic;

namespace TP4SCS.Library.Models.Data;

public partial class Feedback
{
    public int Id { get; set; }

    public int OrderItemId { get; set; }

    public decimal Rating { get; set; }

    public string? Content { get; set; }

    public virtual ICollection<AssetUrl> AssetUrls { get; set; } = new List<AssetUrl>();

    public virtual OrderDetail OrderItem { get; set; } = null!;
}
