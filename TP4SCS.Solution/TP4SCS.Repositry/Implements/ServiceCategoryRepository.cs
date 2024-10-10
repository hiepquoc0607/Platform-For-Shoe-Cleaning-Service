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
    public class ServiceCategoryRepository : BaseRepository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddCategory(ServiceCategory category)
        {
            await Insert(category);
        }

        public async Task DeleteCategory(int id)
        {
            await Delete(id);
        }

        public Task<IEnumerable<ServiceCategory>> GetCategories(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name")
        {
            Expression<Func<ServiceCategory, bool>> filter = s =>
               string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword);

            Func<IQueryable<ServiceCategory>, IOrderedQueryable<ServiceCategory>> orderByExpression = q => orderBy.ToLower() switch
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

        public async Task<ServiceCategory?> GetCategoryById(int id)
        {
            return await GetByID(id);
        }

        public async Task UpdateCategory(ServiceCategory category)
        {
            await Update(category);
        }
    }
}
