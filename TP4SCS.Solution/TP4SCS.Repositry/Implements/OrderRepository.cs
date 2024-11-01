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
            return await GetByIDAsync(id);
        }

        public Task<IEnumerable<Order>?> GetOrdersAsync(
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            // Filter by status and accountId
            Expression<Func<Order, bool>> filter = o =>
                (string.IsNullOrEmpty(status) || o.Status.ToLower().Trim()==status.ToLower().Trim());

            // Sort based on OrderByEnum
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderByExpression = q => orderBy switch
            {
                OrderedOrderByEnum.IdDesc => q.OrderByDescending(o => o.Id),
                OrderedOrderByEnum.IdAsc => q.OrderBy(o => o.Id),
                OrderedOrderByEnum.CreateDateDes => q.OrderByDescending(o => o.CreateTime),
                _ => q.OrderBy(o => o.CreateTime)
            };

            // Check if both pageIndex and pageSize are provided
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                return GetAsync(
                    filter: filter,
                    orderBy: orderByExpression,
                    includeProperties: "OrderDetails",
                    pageIndex: pageIndex.Value,
                    pageSize: pageSize.Value
                );
            }
            else
            {
                // Fetch all orders if pagination parameters are not provided
                return GetAsync(
                    filter: filter,
                    includeProperties: "OrderDetails",
                    orderBy: orderByExpression
                );
            }
        }





        public async Task UpdateOrderAsync(Order order)
        {
            await UpdateAsync(order);
        }
    }
}
