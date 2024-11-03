using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.General;
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
                .ThenInclude(bs => bs.Branch) // Bao gồm Branch trong BranchServices
                .Include(s => s.Category)
                .SingleOrDefaultAsync(s => s.Id == id);
        }


        public async Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc)
        {
            // Xây dựng bộ lọc
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower().Trim() == status.ToLower().Trim());

            // Sắp xếp theo OrderByEnum
            Func<IQueryable<Service>, IOrderedQueryable<Service>> orderByExpression = q => orderBy switch
            {
                OrderByEnum.IdDesc => q.OrderByDescending(c => c.Id),
                _ => q.OrderBy(c => c.Id)
            };

            // Bắt đầu truy vấn với bộ lọc và sắp xếp
            var query = _dbSet.Where(filter);

            // Bao gồm các thuộc tính liên quan
            query = query
                .Include(s => s.Promotion)
                .Include(s => s.AssetUrls)
                .Include(s => s.Category)
                .Include(s => s.BranchServices) // Bao gồm BranchServices
                    .ThenInclude(bs => bs.Branch); // Bao gồm Branch trong BranchServices

            // Thực hiện phân trang nếu có pageIndex và pageSize
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return await query.ToListAsync();
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

        public async Task<(IEnumerable<Service>?, Pagination)> GetServiceByBusinessIdAsync(GetBusinessServiceRequest getBranchServiceRequest)
        {
            int[] branchIds = await _dbContext.BusinessBranches
                .AsNoTracking()
                .Where(b => b.BusinessId == getBranchServiceRequest.BusinessId)
                .Select(b => b.Id)
                .ToArrayAsync();

            var services = _dbContext.Services
                .Include(s => s.BranchServices)
                .ThenInclude(s => s.Branch)
                .Where(bs => branchIds.Contains(bs.Id))
                .Include(s => s.Category)
                .Include(s => s.Promotion)
                .Include(s => s.AssetUrls)
                .OrderBy(s => s.OrderedNum)
                .ThenBy(s => s.CreateTime)
                .AsQueryable();

            //Search
            if (!string.IsNullOrEmpty(getBranchServiceRequest.SearchKey))
            {
                string searchKey = getBranchServiceRequest.SearchKey;
                services = services.Where(s => EF.Functions.Like(s.Name, $"%{searchKey}%"));
            }

            //Category Sort
            if (!string.IsNullOrEmpty(getBranchServiceRequest.Category))
            {
                string cateKey = getBranchServiceRequest.Category;
                services = services
                    .Where(s => EF.Functions
                    .Collate(s.Category.Name, "SQL_Latin1_General_CP1_CI_AI")
                    .Contains(cateKey));
            }

            //Order Sort
            if (!string.IsNullOrEmpty(getBranchServiceRequest.SortBy))
            {
                services = getBranchServiceRequest.SortBy.ToUpper() switch
                {
                    "NAME" => getBranchServiceRequest.IsDecsending
                                ? services.OrderByDescending(s => s.Name)
                                : services.OrderBy(s => s.Name),
                    "PRICE" => getBranchServiceRequest.IsDecsending
                                  ? services.OrderByDescending(s => s.Price)
                                  : services.OrderBy(s => s.Price),
                    "CREATE" => getBranchServiceRequest.IsDecsending
                                  ? services.OrderByDescending(s => s.CreateTime)
                                  : services.OrderBy(s => s.CreateTime),
                    _ => services
                };
            }

            //Paging
            int skipNum = (getBranchServiceRequest.PageNum - 1) * getBranchServiceRequest.PageSize;
            services = services.Skip(skipNum).Take(getBranchServiceRequest.PageSize);

            //Paging Data Calulation
            var data = services;
            var result = await services.ToListAsync();
            int totalData = await data.AsNoTracking().CountAsync();
            int totalPage = (int)Math.Ceiling((decimal)totalData / getBranchServiceRequest.PageSize);

            var paging = new Pagination(totalData, getBranchServiceRequest.PageSize, getBranchServiceRequest.PageNum, totalPage);

            return (result, paging);
        }
    }
}
