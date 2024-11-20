namespace TP4SCS.Library.Models.Data;

public partial class BusinessProfile
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string CitizenId { get; set; } = null!;

    public string FrontCitizenImageUrl { get; set; } = null!;

    public string BackCitizenImageUrl { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public decimal Rating { get; set; }

    public int Rank { get; set; }

    public int TotalOrder { get; set; }

    public int PendingAmount { get; set; }

    public int ProcessingAmount { get; set; }

    public int FinishedAmount { get; set; }

    public int CanceledAmount { get; set; }

    public int ToTalServiceNum { get; set; }

    public DateOnly CreatedDate { get; set; }

    public DateTime RegisteredTime { get; set; }

    public DateTime ExpiredTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<AssetUrl> AssetUrls { get; set; } = new List<AssetUrl>();

    public virtual ICollection<BusinessBranch> BusinessBranches { get; set; } = new List<BusinessBranch>();

    public virtual Account Owner { get; set; } = null!;
}
