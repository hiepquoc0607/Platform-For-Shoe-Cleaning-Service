using System;
using System.Collections.Generic;

namespace TP4SCS.Library.Models.Data;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Order Order { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
