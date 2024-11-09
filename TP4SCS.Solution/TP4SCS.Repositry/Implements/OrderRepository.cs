using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddOrdersAsync(List<Order> orders)
        {
            await _dbContext.AddRangeAsync(orders);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            await DeleteAsync(id);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _dbContext.Orders
                    .Include(o => o.Account)
                        .ThenInclude(a => a.AccountAddresses)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.Promotion)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.Category)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.AssetUrls)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.BranchServices)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Branch)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Material)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Feedbacks)
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>?> GetOrdersAsync(
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            // Filter by status and accountId
            Expression<Func<Order, bool>> filter = o =>
                (string.IsNullOrEmpty(status) || o.Status.ToLower().Trim() == status.ToLower().Trim());

            // Sort based on OrderByEnum
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderByExpression = q => orderBy switch
            {
                OrderedOrderByEnum.IdDesc => q.OrderByDescending(o => o.Id),
                OrderedOrderByEnum.IdAsc => q.OrderBy(o => o.Id),
                OrderedOrderByEnum.CreateDateDes => q.OrderByDescending(o => o.CreateTime),
                _ => q.OrderBy(o => o.CreateTime)
            };

            var query = _dbSet.Where(filter);

            // Bao gồm các thuộc tính liên quan
            query = query
                    .Include(o => o.Account)
                        .ThenInclude(a => a.AccountAddresses)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.Promotion)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.Category)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.AssetUrls)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Service)
                            .ThenInclude(s => s!.BranchServices)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Branch)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Material)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Feedbacks);

            // Thực hiện phân trang nếu có pageIndex và pageSize
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await UpdateAsync(order);
        }
    }
}
