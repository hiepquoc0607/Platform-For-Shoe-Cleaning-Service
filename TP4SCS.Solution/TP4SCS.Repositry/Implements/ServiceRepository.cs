using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class ServiceRepository : BaseRepository<Service>, IServiceRepository
    {
        public ServiceRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {

        }

        public async Task AddService(Service service)
        {
            await Insert(service);
            await SaveAsync();
        }

        public async Task DeleteService(int id)
        {
            await Delete(id);
            await SaveAsync();
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await GetByID(id);
        }

        public Task<IEnumerable<Service>> GetServices(
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
            return Get(
                filter: filter,
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        public async Task UpdateService(Service service)
        {
            await Update(service);
            await SaveAsync();
        }
    }
}
