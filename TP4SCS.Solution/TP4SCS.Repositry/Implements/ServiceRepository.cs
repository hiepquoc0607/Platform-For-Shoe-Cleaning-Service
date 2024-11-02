using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task AddServicesAsync(List<Service> services)
        {
            await _dbSet.AddRangeAsync(services);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddServiceAsync(int[] branchIds, int businessId, Service service)
        {
            // Kiểm tra trạng thái của service
            if (service.Status.ToLower() == StatusConstants.Unavailable.ToLower())
            {
                // Lấy tất cả BranchService có ServiceId tương ứng
                var existingBranchServices = await _dbContext.BranchServices
                    .Where(bs => bs.ServiceId == service.Id)
                    .ToListAsync();

                // Cập nhật trạng thái của tất cả BranchService thành Unavailable
                foreach (var branchService in existingBranchServices)
                {
                    branchService.Status = StatusConstants.Unavailable.ToUpper();
                }
                // Lưu lại thay đổi vào cơ sở dữ liệu
                _dbContext.BranchServices.UpdateRange(existingBranchServices);
                await _dbContext.SaveChangesAsync();
                return; // Kết thúc hàm nếu service là Unavailable
            }
            // Lấy tất cả các branch từ businessId
            var branches = await _dbContext.BusinessBranches
                                           .Where(b => b.BusinessId == businessId)
                                           .ToListAsync();

            // Thêm service mới vào cơ sở dữ liệu
            await _dbContext.Services.AddAsync(service);
            await _dbContext.SaveChangesAsync();

            // Tạo BranchService cho mỗi branch và liên kết với service vừa tạo
            foreach (var branch in branches)
            {
                var branchService = new BranchService
                {
                    ServiceId = service.Id,
                    BranchId = branch.Id,
                    Status = StatusConstants.Unavailable.ToUpper()
                };

                if (branchIds.Contains(branch.Id) && service.Status.ToLower() == StatusConstants.Available.ToLower())
                {
                    branchService.Status = StatusConstants.Available.ToUpper();
                }

                await _dbContext.BranchServices.AddAsync(branchService);
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteServiceAsync(int id)
        {
            var branchServices = await _dbContext.BranchServices
                                .Where(bs => bs.ServiceId == id)
                                .ToListAsync();
            if (branchServices.Any())
            {
                _dbContext.BranchServices.RemoveRange(branchServices);
                await _dbContext.SaveChangesAsync();
            }
            await DeleteAsync(id);
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await _dbContext.Services
                .Include(s => s.Promotion)
                .Include(s => s.AssetUrls)
                .Include(s => s.BranchServices)
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
                    includeProperties: "Promotion,AssetUrls,BranchServices",
                    orderBy: orderByExpression,
                    pageIndex: pageIndex.Value,
                    pageSize: pageSize.Value
                );
            }

            // Fetch all services without pagination
            return GetAsync(
                filter: filter,
                includeProperties: "Promotion,AssetUrls,BranchServices",
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

        public async Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null)
        {
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower() == status.ToLower());

            return await _dbContext.Services.AsNoTracking().CountAsync(filter);
        }

        public async Task UpdateServiceAsync(Service service, int[] branchIds)
        {
            var existingBranchServices = await _dbContext.BranchServices
                                .Where(bs => bs.ServiceId == service.Id)
                                .ToListAsync();
            if (service.Status.ToLower() == StatusConstants.Unavailable.ToLower())
            {
                // Cập nhật trạng thái của tất cả BranchService thành Unavailable
                foreach (var branchService in existingBranchServices)
                {
                    branchService.Status = StatusConstants.Unavailable.ToUpper();
                }
                // Lưu lại thay đổi vào cơ sở dữ liệu
                _dbContext.BranchServices.UpdateRange(existingBranchServices);
                await _dbContext.SaveChangesAsync();
                return; // Kết thúc hàm nếu service là Unavailable
            }


            foreach (var branchService in existingBranchServices)
            {
                // Kiểm tra xem BusinessId có tồn tại trong serviceUpdateRequest hay không
                if (branchIds.Contains(branchService.BranchId))
                {
                    branchService.Status = StatusConstants.Available;
                }
                else
                {
                    branchService.Status = StatusConstants.Unavailable;
                }
            }

            await UpdateAsync(service);
        }
    }
}
