using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddFeedbacksAsync(Feedback feedback)
        {
            await InsertAsync(feedback);
            var orderDetail = await _dbContext.OrderDetails.SingleOrDefaultAsync(od => od.Id == feedback.OrderItemId);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy OrderDetail với ID {feedback.OrderItemId}.");
            }
            var service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == orderDetail.ServiceId);
            if (service == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy Service với ID {orderDetail.ServiceId}.");
            }

            service.Rating = (service.Rating * service.FeedbackedNum + feedback.Rating) / (service.FeedbackedNum + 1);
            service.FeedbackedNum += 1;

            _dbContext.Services.Update(service);
            await _dbContext.SaveChangesAsync();

        }
    }
}
