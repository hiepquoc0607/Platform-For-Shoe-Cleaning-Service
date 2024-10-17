using System;
using System.Collections.Generic;

namespace TP4SCS.Library.Models.Data;

public partial class Statistic
{
    public int Id { get; set; }

    public int BusinessProfileId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal Value { get; set; }

    public decimal Raise { get; set; }

    public bool IsHighest { get; set; }

    public bool IsLowest { get; set; }

    public string Type { get; set; } = null!;
}
