using TP4SCS.Library.Models.Data;

namespace TP4SCS.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task AddOrderDetailsAsync(List<OrderDetail> orderDetails);
        Task<OrderDetail?> GetOrderDetailByIdAsync(int id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task UpdateOrderDetailAsync(OrderDetail orderDetail, int existingOrderDetailId);
        Task DeleteOrderDetailAsync(int id);
    }
}
