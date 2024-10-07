namespace TP4SCS.Library.Models.Data;

public partial class BusinessBranch
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string BranchName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? OrderManagerIds { get; set; }

    public string? EmployeeManagerIds { get; set; }

    public string? CategoryManagerIds { get; set; }

    public string? ServiceManagerIds { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Owner { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
