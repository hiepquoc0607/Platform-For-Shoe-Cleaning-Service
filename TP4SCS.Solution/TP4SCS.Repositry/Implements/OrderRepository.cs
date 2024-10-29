using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddOrderAsync(Order order)
        {
            await InsertAsync(order);
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
            int? accountId = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            // Filter by status and accountId
            Expression<Func<Order, bool>> filter = o =>
                (string.IsNullOrEmpty(status) || Util.IsEqual(o.Status, status)) &&
                (!accountId.HasValue || o.AccountId == accountId);

            // Sort based on OrderByEnum
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(o => o.Id),
                _ => q.OrderBy(o => o.Id)
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
