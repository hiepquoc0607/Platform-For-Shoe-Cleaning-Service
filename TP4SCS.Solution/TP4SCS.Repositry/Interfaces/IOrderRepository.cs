using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrdersAsync(List<Order> orders);

        Task DeleteOrderAsync(int id);

        Task<Order?> GetOrderByIdAsync(int id);

        Task<IEnumerable<Order>?> GetOrdersAsync(
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc);

        Task UpdateOrderAsync(Order order);

        Task<(int, int)> GetBranchIdAndBusinessIdByOrderId(int id);

        Task<Order?> GetOrderByCodeAsync(string code);

        Task<int> CountMonthOrderByBusinessId(int id);

        Task<int> CountYearOrderByBusinessId(int id);
    }
}
