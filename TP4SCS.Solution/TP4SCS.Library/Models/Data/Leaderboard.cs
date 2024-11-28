namespace TP4SCS.Library.Models.Data;

public partial class Leaderboard
{
    public int Id { get; set; }

    public int BusinessId { get; set; }

    public int Rank { get; set; }

    public int Date { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public virtual BusinessProfile Business { get; set; } = null!;
}
