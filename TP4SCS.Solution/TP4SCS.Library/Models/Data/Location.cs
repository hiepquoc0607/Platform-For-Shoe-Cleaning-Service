namespace TP4SCS.Library.Models.Data;

public partial class Location
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Province { get; set; } = null!;
}
