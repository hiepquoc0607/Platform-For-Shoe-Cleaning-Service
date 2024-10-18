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
            await _dbContext.AddRangeAsync(services);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            await DeleteAsync(id);
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await GetByIDAsync(id);
        }

        public Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            int pageIndex = 1,
            int pageSize = 5,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            // Biểu thức lọc
            Expression<Func<Service, bool>> filter = s =>
                string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword);

            Func<IQueryable<Service>, IOrderedQueryable<Service>> orderByExpression = q => orderBy switch
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


        public async Task UpdateServiceAsync(Service service)
        {
            await UpdateAsync(service);
        }
    }
}
