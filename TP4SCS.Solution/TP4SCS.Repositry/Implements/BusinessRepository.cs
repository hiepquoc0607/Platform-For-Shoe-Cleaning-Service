using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class BusinessRepository : GenericRepository<BusinessProfile>, IBusinessRepository
    {
        public BusinessRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task CreateBusinessProfileAsync(BusinessProfile businessProfile)
        {
            await InsertAsync(businessProfile);
        }

        public async Task<(IEnumerable<BusinessProfile>?, Pagination)> GetBusinessesProfilesAsync(GetBusinessRequest getBusinessRequest)
        {
            var businesses = _dbContext.BusinessProfiles.AsQueryable();

            //Search
            if (!string.IsNullOrEmpty(getBusinessRequest.SearchKey))
            {
                string searchKey = getBusinessRequest.SearchKey;
                businesses = businesses.Where(b => EF.Functions.Like(b.Name, $"%{searchKey}%"));
            }

            //Status Filter
            if (!string.IsNullOrEmpty(getBusinessRequest.Status))
            {
                businesses = businesses.Where(b => b.Status.Equals(getBusinessRequest.Status));
            }

            //Order Sort
            if (!string.IsNullOrEmpty(getBusinessRequest.SortBy))
            {
                businesses = getBusinessRequest.SortBy.ToUpper() switch
                {
                    "NAME" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Name)
                                : businesses.OrderBy(b => b.Name),
                    "RATING" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Rating)
                                : businesses.OrderBy(b => b.Rating),
                    "RANK" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Rank)
                                : businesses.OrderBy(b => b.Rank),
                    "TOTAL" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.TotalOrder)
                                : businesses.OrderBy(b => b.TotalOrder),
                    "PENDING" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.PendingAmount)
                                : businesses.OrderBy(b => b.PendingAmount),
                    "PROCESSING" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.ProcessingAmount)
                                : businesses.OrderBy(b => b.ProcessingAmount),
                    "FINISHED" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.FinishedAmount)
                                : businesses.OrderBy(b => b.FinishedAmount),
                    "CANCEL" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.CanceledAmount)
                                : businesses.OrderBy(b => b.CanceledAmount),
                    "STATUS" => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Status)
                                : businesses.OrderBy(b => b.Status),
                    _ => businesses
                };
            }

            //Paging
            int skipNum = (getBusinessRequest.PageNum - 1) * getBusinessRequest.PageSize;
            businesses = businesses.Skip(skipNum).Take(getBusinessRequest.PageSize);

            //Paging Data Calulation
            var data = businesses;
            var result = await businesses.ToListAsync();
            int totalData = await data.AsNoTracking().CountAsync();
            int totalPage = (int)Math.Ceiling((decimal)totalData / getBusinessRequest.PageSize);

            var paging = new Pagination(totalData, getBusinessRequest.PageSize, getBusinessRequest.PageNum, totalPage);

            return (result, paging);
        }

        public async Task<int?> GetBusinessIdByOwnerIdAsync(int id)
        {
            return await _dbContext.BusinessProfiles.AsNoTracking()
                .Where(p => p.OwnerId == id)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<BusinessProfile?> GetBusinessByOwnerIdAsync(int id)
        {
            return await _dbContext.BusinessProfiles.Where(p => p.OwnerId == id).FirstOrDefaultAsync();
        }

        public async Task<BusinessProfile?> GetBusinessProfileByIdAsync(int id)
        {
            return await _dbContext.BusinessProfiles.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetBusinessProfileMaxIdAsync()
        {
            return await _dbContext.BusinessProfiles.AsNoTracking().MaxAsync(p => p.Id);
        }

        public async Task<bool> IsNameExistedAsync(string name)
        {
            return await _dbContext.BusinessProfiles.AnyAsync(b => string.Equals(b.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> IsPhoneExistedAsync(string phone)
        {
            return await _dbContext.BusinessProfiles.AnyAsync(b => string.Equals(b.Phone, phone, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateBusinessProfileAsync(BusinessProfile businessProfile)
        {
            await UpdateAsync(businessProfile);
        }

        public async Task<int> CountBusinessServiceByIdAsync(int id)
        {
            return await _dbContext.Services
                .AsNoTracking()
                .GroupBy(s => new { s.Branch.BusinessId, s.Name })
                .Select(s => new { s.Key.BusinessId, ServiceName = s.Key.Name })
                .GroupBy(s => s.BusinessId)
                .Select(s => new
                {
                    BusinessId = s.Key,
                    DistinctService = s.ToList()
                })
                .CountAsync();
        }
    }
}
