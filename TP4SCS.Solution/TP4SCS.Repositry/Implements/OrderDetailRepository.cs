using Google.Apis.Storage.v1.Data;
using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Response.Location;
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
            var existingOrderDetail = order.OrderDetails.SingleOrDefault(od => od.ServiceId!.Value == orderDetail.ServiceId
             && od.BranchId == orderDetail.BranchId);
            if (existingOrderDetail != null)
            {
                existingOrderDetail.Quantity += orderDetail.Quantity;
                existingOrderDetail.Status = StatusConstants.PROCESSING;
            }
            else
            {
                var service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == orderDetail.ServiceId!.Value);
                if (service == null)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {orderDetail.ServiceId} không tìm thấy.");
                }
                if (service.Status.ToUpper() == StatusConstants.INACTIVE)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {orderDetail.ServiceId} đã ngừng hoạt động.");
                }
                order.OrderDetails.Add(orderDetail);
            }
            order.Status = StatusConstants.PROCESSING;
            order.OrderPrice = order.OrderDetails.Sum(od => od.Quantity * od.Price);
            order.TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.Price) + order.DeliveredFee;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            var od = await _dbContext.OrderDetails
                .Include(od => od.Feedbacks)
                .SingleOrDefaultAsync(_ => _.Id == id);
            if (od == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy OrderDetail với ID {id}.");
            }
            if (od.Feedbacks != null)
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
                order.OrderPrice = order.OrderDetails.Sum(od => od.Quantity * od.Price);
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
                .Include(od => od.Branch)
                .Include(od => od.Material)
                .Include(od => od.Service)
                .ToListAsync();
        }


        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            // Lấy OrderDetail hiện tại từ database
            var existingOrderDetail = await _dbContext.OrderDetails
                .SingleOrDefaultAsync(od => od.Id == orderDetail.Id);

            if (existingOrderDetail == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy OrderDetail với ID {orderDetail.Id}.");
            }


            // Cập nhật OrderDetail
            _dbContext.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
            await _dbContext.SaveChangesAsync();

            // Lấy Order liên quan
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .SingleOrDefaultAsync(o => o.Id == orderDetail.OrderId);

            if (order != null)
            {
                order.OrderPrice = order.OrderDetails.Sum(od => od.Quantity * od.Price);
                order.TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.Price) + order.DeliveredFee;
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
