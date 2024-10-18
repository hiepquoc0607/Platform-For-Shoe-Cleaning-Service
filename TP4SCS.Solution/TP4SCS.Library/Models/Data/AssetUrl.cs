namespace TP4SCS.Library.Models.Data;

public partial class AssetUrl
{
    public int Id { get; set; }

    public int? BusinessProfileId { get; set; }

    public int? FeedbackId { get; set; }

    public int? ServiceId { get; set; }

    public string Url { get; set; } = null!;

    public bool IsImage { get; set; }

    public string Type { get; set; } = null!;

    public virtual BusinessProfile? BusinessProfile { get; set; }

    public virtual Feedback? Feedback { get; set; }

    public virtual Service? Service { get; set; }
}
