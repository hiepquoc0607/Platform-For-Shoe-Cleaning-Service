using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class ServiceRepository : GenericRepoistory<Service>, IServiceRepository
    {
        public ServiceRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {

        }

        public async Task AddService(Service service)
        {
            await Insert(service);
        }

        public async Task DeleteService(int id)
        {
            await Delete(id);
        }

        public async Task<Service?> GetServiceById(int id)
        {
            return await GetByIDAsync(id);
        }

        public Task<IEnumerable<Service>?> GetServices(
        string? keyword = null,
        int pageIndex = 1,
        int pageSize = 5,
        string orderBy = "name")
        {
            Expression<Func<Service, bool>> filter = s =>
                string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword);

            Func<IQueryable<Service>, IOrderedQueryable<Service>> orderByExpression = q => orderBy.ToLower() switch
            {
                "namedesc" => q.OrderByDescending(c => c.Name),
                _ => q.OrderBy(c => c.Name)
            };
            return GetAsync(
                filter: filter,
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        public async Task UpdateService(Service service)
        {
            await Update(service);
        }
    }
}
