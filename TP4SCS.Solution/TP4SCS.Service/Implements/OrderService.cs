using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Order;
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
        public async Task<Order?> GetOrderByOrderId(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
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

        public async Task UpdateOrderAsync(int existingOrderId, UpdateOrderRequest request)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(existingOrderId);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {existingOrderId}");
            }

            // Cập nhật thông tin ShippingUnit và ShippingCode nếu có
            if (request.ShippingUnit != null)
            {
                existingOrder.ShippingUnit = request.ShippingUnit;
            }

            if (request.ShippingCode != null)
            {
                existingOrder.ShippingCode = request.ShippingCode;
            }

            // Cập nhật DeliveredFee nếu có giá trị
            if (request.DeliveredFee.HasValue)
            {
                existingOrder.DeliveredFee = request.DeliveredFee.Value;
            }

            // Cập nhật các thời gian nếu không phải null
            if (request.PendingTime.HasValue)
            {
                existingOrder.PendingTime = request.PendingTime.Value;
            }

            if (request.ApprovedTime.HasValue)
            {
                existingOrder.ApprovedTime = request.ApprovedTime.Value;
            }

            if (request.RevievedTime.HasValue)
            {
                existingOrder.RevievedTime = request.RevievedTime.Value;
            }

            if (request.ProcessingTime.HasValue)
            {
                existingOrder.ProcessingTime = request.ProcessingTime.Value;
            }

            if (request.StoragedTime.HasValue)
            {
                existingOrder.StoragedTime = request.StoragedTime.Value;
            }

            if (request.ShippingTime.HasValue)
            {
                existingOrder.ShippingTime = request.ShippingTime.Value;
            }

            if (request.DeliveredTime.HasValue)
            {
                existingOrder.DeliveredTime = request.DeliveredTime.Value;
            }

            if (request.FinishedTime.HasValue)
            {
                existingOrder.FinishedTime = request.FinishedTime.Value;
            }

            if (request.AbandonedTime.HasValue)
            {
                existingOrder.AbandonedTime = request.AbandonedTime.Value;
            }

            // Cập nhật đơn hàng trong cơ sở dữ liệu
            await _orderRepository.UpdateOrderAsync(existingOrder);
        }


    }
}
