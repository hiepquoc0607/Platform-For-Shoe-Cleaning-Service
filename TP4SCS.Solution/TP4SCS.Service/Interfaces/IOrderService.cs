using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>?> GetOrdersByBranchIdAsync(
            int branchId,
            string? status = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc);
        Task<IEnumerable<Order>?> GetOrdersByAccountIdAsync(
            int accountId,
            string? status = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc);
        Task<IEnumerable<Order>?> GetOrdersAsync(
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc);
        Task UpdateOrderStatusAsync(int existingOrderedId, string newStatus);
        Task ApprovedOrder(int orderId);
    }
}
