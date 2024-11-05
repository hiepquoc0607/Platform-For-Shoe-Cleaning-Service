using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils.StaticClass;
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
            return orders.Where(o => o.OrderDetails.Any(od => od.BranchId == branchId));
        }
        public async Task<IEnumerable<Order>?> GetOrdersByBusinessIdAsync(
            int businessId,
            string? status = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            var orders = await GetOrdersAsync(status, null, null, orderBy);
            if (orders == null)
            {
                return Enumerable.Empty<Order>();
            }
            return orders.Where(o => o.OrderDetails.Any(od => od.Branch.BusinessId == businessId));
        }
        public async Task ApprovedOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                throw new InvalidOperationException($"Đơn hàng với ID {orderId} không tồn tại.");
            }

            if (!Util.IsEqual(order.Status, StatusConstants.PENDING))
            {
                throw new InvalidOperationException($"Đơn hàng với ID {orderId} không ở trạng thái chờ duyệt.");
            }

            await UpdateOrderStatusAsync(orderId, StatusConstants.APPROVED);
        }

        public async Task UpdateOrderStatusAsync(int existingOrderedId, string newStatus)
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

            order.Status = newStatus.ToUpper();

            await _orderRepository.UpdateOrderAsync(order);
        }

    }
}
