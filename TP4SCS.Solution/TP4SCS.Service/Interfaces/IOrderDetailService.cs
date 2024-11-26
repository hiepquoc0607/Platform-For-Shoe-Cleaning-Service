using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.OrderDetail;

namespace TP4SCS.Services.Interfaces
{
    public interface IOrderDetailService
    {
 //       Task AddOrderDetailsAsync(List<OrderDetail> orderDetails);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task<OrderDetail?> GetOrderDetailByIdAsync(int id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task DeleteOrderDetailAsync(int id);
    }
}
