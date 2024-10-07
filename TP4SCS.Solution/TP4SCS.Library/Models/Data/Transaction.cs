namespace TP4SCS.Library.Models.Data;

public partial class Transaction
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int MethodId { get; set; }

    public int? PackId { get; set; }

    public decimal Balance { get; set; }

    public DateTime ProcessTime { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual PaymentMethod Method { get; set; } = null!;

    public virtual SubscriptionPack? Pack { get; set; }
}
