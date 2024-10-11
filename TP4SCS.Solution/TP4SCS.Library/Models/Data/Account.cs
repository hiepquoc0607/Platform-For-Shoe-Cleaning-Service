namespace TP4SCS.Library.Models.Data;

public partial class Account
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public DateTime? ExpiredTime { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsGoogle { get; set; }

    public string? Fcmtoken { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<AccountAddress> AccountAddresses { get; set; } = new List<AccountAddress>();

    public virtual ICollection<BusinessBranch> BusinessBranches { get; set; } = new List<BusinessBranch>();

    public virtual ICollection<BusinessProfile> BusinessProfiles { get; set; } = new List<BusinessProfile>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
