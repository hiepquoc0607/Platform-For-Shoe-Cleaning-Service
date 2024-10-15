namespace TP4SCS.Library.Models.Data;

public partial class Order
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime? DeliveredTime { get; set; }

    public bool IsAutoReject { get; set; }

    public bool IsShipReq { get; set; }

    public string? Note { get; set; }

    public decimal OrderPrice { get; set; }

    public decimal ShipFee { get; set; }

    public decimal TotalPrice { get; set; }

    public string? ShippingUnit { get; set; }

    public string? ShippingCode { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderNotification> OrderNotifications { get; set; } = new List<OrderNotification>();

    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
}
