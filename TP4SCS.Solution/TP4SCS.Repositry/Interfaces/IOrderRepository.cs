using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>?> GetOrdersAsync(
            string? status = null,
            int? accountId = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc);
        Task UpdateOrderAsync(Order order);
    }
}
