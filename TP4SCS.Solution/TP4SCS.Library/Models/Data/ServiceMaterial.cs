namespace TP4SCS.Library.Models.Data;

public partial class ServiceMaterial
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int? MaterialId { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Service Service { get; set; } = null!;
}
