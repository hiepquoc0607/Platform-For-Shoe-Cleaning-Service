namespace TP4SCS.Library.Models.Data;

public partial class AccountAddress
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;
}
