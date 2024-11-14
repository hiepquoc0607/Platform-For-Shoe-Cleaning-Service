using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<Feedback?> GetFeedbackByidAsync(int id)
        {
            return await GetByIDAsync(id);
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

        public Task<IEnumerable<Feedback>?> GetFeedbacksAsync(
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            // Biểu thức lọc với cả keyword và status
            Expression<Func<Feedback, bool>> filter = s =>
                (string.IsNullOrEmpty(status) || s.Status.ToLower() == status.ToLower());

            Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                OrderByEnum.IdAsc => q.OrderBy(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };
            return GetAsync(
                filter: filter,
                includeProperties: "AssetUrls",
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        public async Task<IEnumerable<Feedback>?> GetFeedbacksByServiceIdAsync(
            int serviceId,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            // Biểu thức lọc với cả keyword và status
            Expression<Func<Feedback, bool>> filter = s =>
                (string.IsNullOrEmpty(status) || s.Status.ToLower() == status.ToLower());

            Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };
            var feedbacks = await GetAsync(
                filter: filter,
                includeProperties: "AssetUrls,OrderItem,OrderItem.Order",
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize);
            return feedbacks?.Where(f => f.OrderItem.ServiceId == serviceId);
        }

        public async Task<IEnumerable<Feedback>?> GetFeedbacksByAccountIdAsync(
            int accountId,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            Expression<Func<Feedback, bool>> filter = s =>
                (string.IsNullOrEmpty(status) || s.Status.ToLower().Trim() == status.ToLower().Trim());

            Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };
            var query = _dbSet.Where(filter);

            // Bao gồm các thuộc tính liên quan
            query = query
                .Include(f => f.AssetUrls)
                .Include(f => f.OrderItem)
                    .ThenInclude(od => od.Order);
            return await query.Where(f => f.OrderItem.Order.AccountId == accountId).ToListAsync();
        }
        public async Task<IEnumerable<Feedback>?> GetFeedbacksByBranchIdIdAsync(
            int branchId,
            string? status = null,
            OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            Expression<Func<Feedback, bool>> filter = s =>
                (string.IsNullOrEmpty(status) || s.Status.ToLower().Trim() == status.ToLower().Trim());

            Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };
            var query = _dbSet.Where(filter);

            // Bao gồm các thuộc tính liên quan
            query = query
                .Include(f => f.AssetUrls)
                .Include(f => f.OrderItem)
                    .ThenInclude(od => od.Order);
            return await query.Where(f => f.OrderItem.BranchId == branchId).ToListAsync();
        }
        public async Task DeleteFeedbackAsync(int id)
        {
            // Kiểm tra nếu Feedback tồn tại
            var feedback = await _dbContext.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                throw new KeyNotFoundException($"Feedback with ID {id} not found.");
            }

            // Xóa AssetUrls liên quan
            var assetUrlsToRemove = _dbContext.AssetUrls.Where(a => a.FeedbackId == id).ToList();
            _dbContext.AssetUrls.RemoveRange(assetUrlsToRemove);

            // Lưu thay đổi
            await _dbContext.SaveChangesAsync();

            // Xóa Feedback
            await DeleteAsync(id);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            await UpdateAsync(feedback);
        }
    }
}
