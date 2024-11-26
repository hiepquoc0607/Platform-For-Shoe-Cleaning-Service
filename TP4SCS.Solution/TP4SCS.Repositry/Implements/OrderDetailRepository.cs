using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils.StaticClass;
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

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .SingleOrDefaultAsync(c => c.Id == orderDetail.OrderId);
            if (order == null)
            {
                throw new Exception($"Order với id: {orderDetail.OrderId} không tồn tại");
            }

            if (orderDetail.ServiceId.HasValue)
            {
                var service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == orderDetail.ServiceId.Value);
                if (service == null)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {orderDetail.ServiceId} không tìm thấy.");
                }

                if (service.Status.ToUpper() == StatusConstants.UNAVAILABLE)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {orderDetail.ServiceId} đã ngừng hoạt động.");
                }
            }
            if (orderDetail.MaterialId.HasValue)
            {
                var material = await _dbContext.Materials.SingleOrDefaultAsync(m => m.Id == orderDetail.MaterialId.Value);
                if (material == null)
                {
                    throw new InvalidOperationException($"Vật liệu với ID {orderDetail.MaterialId} không tìm thấy.");
                }
            }
            order.OrderDetails.Add(orderDetail);
            order.Status = StatusConstants.PROCESSING;
            order.OrderPrice = order.OrderDetails.Sum(od => od.Price);
            order.TotalPrice = order.OrderDetails.Sum(od => od.Price) + order.DeliveredFee;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            var od = await _dbContext.OrderDetails
                .Include(od => od.Feedback)
                .SingleOrDefaultAsync(_ => _.Id == id);
            if (od == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy OrderDetail với ID {id}.");
            }
            if (od.Feedback != null)
            {
                throw new InvalidOperationException("Không thể xóa OrderDetail vì có liên kết với Feedbacks.");
            }
            var order = await _dbContext.Orders
                             .Include(o => o.OrderDetails)
                             .SingleOrDefaultAsync(c => c.Id == od.OrderId);


            if (order != null)
            {
                if (order.OrderDetails.Count() <= 1)
                {
                    throw new InvalidOperationException("Không thể xóa OrderDetail vì đơn hàng cần ít nhất một chi tiết.");
                }
                await DeleteAsync(id);
                order.OrderPrice = order.OrderDetails.Sum(od => od.Price);
                order.TotalPrice = order.OrderPrice + order.DeliveredFee;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
        {
            return await _dbContext.OrderDetails
                .Include(od => od.Branch)
                .Include(od => od.Material)
                .Include(od => od.Service)
                .ThenInclude(s => s!.Promotion)
                .SingleOrDefaultAsync(od => od.Id == id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _dbContext.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Order)
                .Include(od => od.Branch)
                .Include(od => od.Material)
                .Include(od => od.Service)
                .Select(od => new OrderDetail
                {
                    Id = od.Id,
                    Order = od.Order,
                    Branch = od.Branch,
                    Service = od.Service,
                    Material = od.Material,
                    Price = od.Price
                })
                .ToListAsync();
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            await UpdateAsync(orderDetail);
        }

    }
}
