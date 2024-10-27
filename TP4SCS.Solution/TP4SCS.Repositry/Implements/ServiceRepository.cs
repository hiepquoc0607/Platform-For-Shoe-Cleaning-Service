using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class ServiceRepository : GenericRepoistory<Service>, IServiceRepository
    {
        public ServiceRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddServiceAsync(List<Service> services)
        {
            await _dbSet.AddRangeAsync(services);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            await DeleteAsync(id);
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await _dbContext.Services
                .Include(s => s.Promotion)
                .SingleOrDefaultAsync(s => s.Id == id);
        }


        public Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            string? status = null,
            int pageIndex = 1,
            int pageSize = 5,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower() == status.ToLower());

            Func<IQueryable<Service>, IOrderedQueryable<Service>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };

            return GetAsync(
                filter: filter,
                includeProperties: "Promotion",
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }


        public async Task<IEnumerable<Service>> GetServicesAsync(string? keyword = null, string? status = null)
        {
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower() == status.ToLower());

            return await _dbContext.Services
                .AsNoTracking()
                .Include(s => s.Promotion)
                .Where(filter)
                .ToListAsync();
        }

        public async Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null)
        {
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower() == status.ToLower());

            return await _dbContext.Services.AsNoTracking().CountAsync(filter);
        }

        public async Task UpdateServiceAsync(Service service)
        {
            await UpdateAsync(service);
        }
    }
}
