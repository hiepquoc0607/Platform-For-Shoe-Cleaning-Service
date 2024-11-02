using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
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
                .Include(s => s.AssetUrls)
                .SingleOrDefaultAsync(s => s.Id == id);
        }


        public Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower().Trim() == status.ToLower().Trim());

            // Sort based on OrderByEnum
            Func<IQueryable<Service>, IOrderedQueryable<Service>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };

            // Check for pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Fetch paginated services
                return GetAsync(
                    filter: filter,
                    includeProperties: "Promotion,AssetUrls",
                    orderBy: orderByExpression,
                    pageIndex: pageIndex.Value,
                    pageSize: pageSize.Value
                );
            }

            // Fetch all services without pagination
            return GetAsync(
                filter: filter,
                includeProperties: "Promotion,AssetUrls",
                orderBy: orderByExpression
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

        public async Task<IEnumerable<Service>> GetServicesIncludeBranchAsync()
        {
            return await _dbContext.Services
                .OrderBy(s => s.Id)
                //.Include(s => s.Branch)
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
