using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Notification;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class OrderNotificationRepository : GenericRepository<OrderNotification>, IOrderNotificationRepository
    {
        public OrderNotificationRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task CreateOrderNotificationAsync(OrderNotification orderNotification)
        {
            await InsertAsync(orderNotification);
        }

        public async Task DeleteOrderNotificationAsync(int id)
        {
            await DeleteAsync(id);
        }

        public async Task<(IEnumerable<OrderNotification>?, Pagination)> GetOrderNotificationsAsync(GetOrderNotificationRequest getOrderNotificationRequest)
        {
            //var notifiesQuery = _dbContext.OrderNotifications
            //    .AsNoTracking()
            //    .Include(n=>n.Order)
            //    .ThenInclude(n=>n.Account)
            //    .Include(n=>n.Order)
            //    .ThenInclude(n=>n.)
            //    .AsQueryable();
            throw new NotImplementedException();
        }
    }
}
