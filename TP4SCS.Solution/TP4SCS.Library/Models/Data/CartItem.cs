using System;
using System.Collections.Generic;

namespace TP4SCS.Library.Models.Data;

public partial class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int ServiceId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
