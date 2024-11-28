namespace TP4SCS.Library.Models.Data;

public partial class BusinessStatistic
{
    public int Id { get; set; }

    public int BusinessId { get; set; }

    public int Value { get; set; }

    public int Date { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public string Type { get; set; } = null!;

    public virtual BusinessProfile Business { get; set; } = null!;
}
