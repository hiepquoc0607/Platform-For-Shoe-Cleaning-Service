using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>?> GetOrdersAsync(string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            return await _orderRepository.GetOrdersAsync(status, pageIndex, pageSize, orderBy);
        }

        public async Task<IEnumerable<Order>?> GetOrdersByAccountIdAsync(
            int accountId,
            string? status = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            var orders = await GetOrdersAsync(status, null, null, orderBy);
            if (orders == null)
            {
                return Enumerable.Empty<Order>();
            }
            // Lọc đơn hàng theo AccountId
            return orders.Where(o => o.AccountId == accountId);
        }

        public async Task<IEnumerable<Order>?> GetOrdersByBranchIdAsync(
            int branchId,
            string? status = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            var orders = await GetOrdersAsync(status, null, null, orderBy);
            if (orders == null)
            {
                return Enumerable.Empty<Order>();
            }
            //return orders.Where(o => o.OrderDetails.Any(od => od.Service.BranchId == branchId));
            return orders;
        }

        public async Task UpdateOrderStatus(int existingOrderedId, string newStatus)
        {
            if (!Util.IsValidOrderStatus(newStatus))
            {
                throw new ArgumentException("Order status không hợp lệ");
            }

            var order = await _orderRepository.GetOrderByIdAsync(existingOrderedId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {existingOrderedId}");
            }

            order.Status = newStatus;

            await _orderRepository.UpdateOrderAsync(order);
        }

    }
}
