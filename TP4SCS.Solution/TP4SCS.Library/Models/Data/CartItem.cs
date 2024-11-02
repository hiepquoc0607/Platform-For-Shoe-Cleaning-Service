namespace TP4SCS.Library.Models.Data;

public partial class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int BranchId { get; set; }

    public int? ServiceId { get; set; }

    public int? MaterialId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual BusinessBranch Branch { get; set; } = null!;

    public virtual Cart Cart { get; set; } = null!;

    public virtual Material? Material { get; set; }

    public virtual Service? Service { get; set; }
}
