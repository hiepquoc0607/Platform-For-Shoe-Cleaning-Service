namespace TP4SCS.Library.Models.Data;

public partial class BusinessBranch
{
    public int Id { get; set; }

    public int BusinessId { get; set; }

    public string BranchName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? EmployeeIds { get; set; }

    public int PendingAmount { get; set; }

    public int ProcessingAmount { get; set; }

    public int FinishedAmount { get; set; }

    public int CanceledAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual BusinessProfile Business { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
