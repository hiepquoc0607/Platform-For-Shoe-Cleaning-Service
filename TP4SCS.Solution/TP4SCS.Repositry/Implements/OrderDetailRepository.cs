using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddOrderDetailsAsync(List<OrderDetail> orderDetails)
        {
            await _dbContext.AddRangeAsync(orderDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            await DeleteAsync(id);
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
        {
            return await GetByIDAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _dbContext.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
           await UpdateAsync(orderDetail);
        }
    }
}
