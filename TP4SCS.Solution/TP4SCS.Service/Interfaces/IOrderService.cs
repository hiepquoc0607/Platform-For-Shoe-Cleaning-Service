using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>?> GetOrdersAsync(
            string? keyword = null,
            string? status = null,
            int? branchId = null,
            int? accountId = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc);
    }
}
