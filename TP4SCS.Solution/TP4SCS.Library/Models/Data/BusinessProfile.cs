namespace TP4SCS.Library.Models.Data;

public partial class BusinessProfile
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public decimal Rating { get; set; }

    public int TotalOrder { get; set; }

    public int PendingAmt { get; set; }

    public int ProcessingAmt { get; set; }

    public int FinishedAmt { get; set; }

    public int CanceledAmt { get; set; }

    public DateTime RegisterTime { get; set; }

    public DateTime ExpireTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<AssetUrl> AssetUrls { get; set; } = new List<AssetUrl>();

    public virtual Account Owner { get; set; } = null!;
}
