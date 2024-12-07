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
        private readonly ILeaderboardRepository _leaderboardRepository;
        public ServiceRepository(ILeaderboardRepository leaderboardRepository, Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
            _leaderboardRepository = leaderboardRepository;
        }

        public async Task AddServicesAsync(List<Service> services)
        {
            await _dbSet.AddRangeAsync(services);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddServiceAsync(int[] branchIds, int businessId, Service service)
        {
            var existingService = await _dbContext.Services
                            .AnyAsync(s => s.Name.ToLower() == service.Name.ToLower() && s.BranchServices.Any(bs => bs.Branch.BusinessId == businessId));

            if (existingService)
            {
                throw new InvalidOperationException($"Service with the name '{service.Name}' already exists for Business ID {businessId}.");
            }
            // Lấy tất cả các branch từ businessId
            var branches = await _dbContext.BusinessBranches
                               .Where(b => b.BusinessId == businessId)
                               .ToListAsync();

            // Thêm service mới vào cơ sở dữ liệu
            await _dbContext.Services.AddAsync(service);
            await _dbContext.SaveChangesAsync();

            // Kiểm tra trạng thái của service
            if (service.Status.ToLower() == StatusConstants.UNAVAILABLE.ToLower())
            {
                // Tạo BranchService cho mỗi branch với trạng thái UNAVAILABLE
                foreach (var branch in branches)
                {
                    var branchService = new BranchService
                    {
                        ServiceId = service.Id,
                        BranchId = branch.Id,
                        Status = StatusConstants.UNAVAILABLE.ToUpper()
                    };
                    await _dbContext.BranchServices.AddAsync(branchService);
                }

                // Lưu lại thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();
                return; // Kết thúc hàm nếu service là Unavailable
            }

            // Tạo BranchService cho mỗi branch và liên kết với service vừa tạo
            foreach (var branch in branches)
            {
                var branchService = new BranchService
                {
                    ServiceId = service.Id,
                    BranchId = branch.Id,
                    Status = StatusConstants.UNAVAILABLE.ToUpper()
                };

                if (branchIds.Contains(branch.Id) && service.Status.ToLower() == StatusConstants.AVAILABLE.ToLower())
                {
                    branchService.Status = StatusConstants.AVAILABLE.ToUpper();
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
                .Include(s => s.ServiceProcesses)
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
            OrderByEnum orderBy = OrderByEnum.Rank)
        {
            // Xây dựng bộ lọc
            Expression<Func<Service, bool>> filter = s =>
                (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)) &&
                (string.IsNullOrEmpty(status) || s.Status.ToLower().Trim() == status.ToLower().Trim());

            // Bắt đầu truy vấn với bộ lọc
            var query = _dbSet.Where(filter);

            // Lấy leaderboard từ repository
            var leaderShops = await _leaderboardRepository.GetLeaderboardByMonthAsync();

            // Kiểm tra nếu leaderShops có ít hơn 3 hoặc null
            if (leaderShops == null || leaderShops.Businesses.Count() < 3)
            {
                throw new Exception("Danh sách cửa hàng đứng đầu không hợp lệ hoặc không đủ dữ liệu. Vui lòng kiểm tra lại.");
            }

            // Lấy 3 cửa hàng đứng đầu
            var top3LeaderShops = leaderShops.Businesses.Take(3).ToList();

            // Áp dụng sắp xếp theo thứ tự cửa hàng trong leaderboard
            var shopIds = top3LeaderShops.Select(shop => shop.Id).ToList();

            // Áp dụng sắp xếp theo Rank, ưu tiên theo ID cửa hàng trong leaderShops


            // Bao gồm các thuộc tính liên quan
            query = query
                .Include(s => s.ServiceProcesses)
                .Include(s => s.Promotion)
                .Include(s => s.AssetUrls)
                .Include(s => s.Category)
                .Include(s => s.BranchServices) // Bao gồm BranchServices
                    .ThenInclude(bs => bs.Branch) // Bao gồm Branch trong BranchServices
                        .ThenInclude(b => b.Business); // Bao gồm Branch trong BranchServices
            var allServices = await query.ToListAsync();
            var orderedServices = allServices.OrderBy(s =>
            {
                var businessId = s.BranchServices.FirstOrDefault()?.Branch?.BusinessId ?? -1;

                // Kiểm tra BusinessId có trong shopIds hay không và trả về thứ tự của nó
                if (shopIds.Contains(businessId))
                {
                    return shopIds.IndexOf(businessId);  // Trả về vị trí của businessId trong shopIds
                }

                // Các dịch vụ có BusinessId không có trong shopIds sẽ được xếp sau, trả về giá trị lớn hơn (int.MaxValue)
                return int.MaxValue;
            }).ToList();

            // Sắp xếp theo các yêu cầu khác nếu có
            if (orderBy == OrderByEnum.IdDesc)
            {
                orderedServices = orderedServices.OrderByDescending(s => s.Id).ToList();
            }
            else if (orderBy == OrderByEnum.IdAsc)
            {
                orderedServices = orderedServices.OrderBy(s => s.Id).ToList();
            }
            // Thực hiện phân trang nếu có pageIndex và pageSize
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                orderedServices = orderedServices.Skip(validPageIndex * validPageSize).Take(validPageSize).ToList();
            }

            // Trả về kết quả
            return orderedServices;
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
            if (service.Status.ToLower() == StatusConstants.UNAVAILABLE.ToLower())
            {
                // Cập nhật trạng thái của tất cả BranchService thành Unavailable
                foreach (var branchService in existingBranchServices)
                {
                    branchService.Status = StatusConstants.UNAVAILABLE.ToUpper();
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
                    branchService.Status = StatusConstants.AVAILABLE;
                }
                else
                {
                    branchService.Status = StatusConstants.UNAVAILABLE;
                }
            }

            await UpdateAsync(service);
        }

        public async Task UpdateServiceAsync(Service service)
        {
            await UpdateAsync(service);
        }
    }
}
