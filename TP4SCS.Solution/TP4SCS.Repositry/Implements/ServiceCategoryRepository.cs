using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class ServiceCategoryRepository : GenericRepoistory<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddCategoryAsync(ServiceCategory category)
        {
            await InsertAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await DeleteAsync(id);
        }

        public Task<IEnumerable<ServiceCategory>?> GetCategoriesAsync(
            string? keyword = null,
            int pageIndex = 1,
            int pageSize = 5,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            // Biểu thức lọc
            Expression<Func<ServiceCategory, bool>> filter = s =>
                string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword);

            Func<IQueryable<ServiceCategory>, IOrderedQueryable<ServiceCategory>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id), // Sắp xếp giảm dần
                _ => q.OrderBy(c => c.Id)                             // Mặc định sắp xếp tăng dần
            };

            // Gọi hàm GetAsync với các tham số đã cập nhật
            return GetAsync(
                filter: filter,
                orderBy: orderByExpression,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }


        public async Task<ServiceCategory?> GetCategoryByIdAsync(int id)
        {
            return await GetByIDAsync(id);
        }

        public async Task UpdateCategoryAsync(ServiceCategory category)
        {
            await UpdateAsync(category);
        }
    }
}
