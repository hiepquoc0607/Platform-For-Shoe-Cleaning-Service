using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class BusinessRepository : GenericRepository<BusinessProfile>, IBusinessRepository
    {
        private readonly IBranchRepository _branchRepository;

        public BusinessRepository(Tp4scsDevDatabaseContext dbContext, IBranchRepository branchRepository) : base(dbContext)
        {
            _branchRepository = branchRepository;
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
            if (getBusinessRequest.Status != null)
            {
                //businesses = businesses.Where(b => b.Status.Equals(getBusinessRequest.Status));
                businesses = getBusinessRequest.Status switch
                {
                    BusinessStatus.ACTIVE => businesses.Where(b => b.Status.Equals(StatusConstants.ACTIVE)),
                    BusinessStatus.INACTIVE => businesses.Where(b => b.Status.Equals(StatusConstants.INACTIVE)),
                    BusinessStatus.SUSPENDED => businesses.Where(b => b.Status.Equals(StatusConstants.SUSPENDED)),
                    _ => businesses
                };
            }

            //Order Sort
            if (getBusinessRequest.SortBy != null)
            {
                businesses = getBusinessRequest.SortBy switch
                {
                    BusinessSortOption.NAME => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Name)
                                : businesses.OrderBy(b => b.Name),
                    BusinessSortOption.RATING => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Rating)
                                : businesses.OrderBy(b => b.Rating),
                    BusinessSortOption.RANK => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Rank)
                                : businesses.OrderBy(b => b.Rank),
                    BusinessSortOption.TOTAL => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.TotalOrder)
                                : businesses.OrderBy(b => b.TotalOrder),
                    BusinessSortOption.PENDING => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.PendingAmount)
                                : businesses.OrderBy(b => b.PendingAmount),
                    BusinessSortOption.PROCESSING => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.ProcessingAmount)
                                : businesses.OrderBy(b => b.ProcessingAmount),
                    BusinessSortOption.FINISHED => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.FinishedAmount)
                                : businesses.OrderBy(b => b.FinishedAmount),
                    BusinessSortOption.CANCEL => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.CanceledAmount)
                                : businesses.OrderBy(b => b.CanceledAmount),
                    BusinessSortOption.STATUS => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Status)
                                : businesses.OrderBy(b => b.Status),
                    _ => businesses
                };
            }

            //Count Total Data
            int totalData = await businesses.AsNoTracking().CountAsync();

            //Paging
            int skipNum = (getBusinessRequest.PageNum - 1) * getBusinessRequest.PageSize;
            businesses = businesses.Skip(skipNum).Take(getBusinessRequest.PageSize);

            //Paging Data Calulation
            var result = await businesses.ToListAsync();
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
            return await _dbContext.BusinessProfiles.SingleOrDefaultAsync(p => p.OwnerId == id);
        }

        public async Task<BusinessProfile?> GetBusinessProfileByIdAsync(int id)
        {
            return await _dbContext.BusinessProfiles.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetBusinessProfileMaxIdAsync()
        {
            return await _dbContext.BusinessProfiles.AsNoTracking().MaxAsync(p => p.Id);
        }

        public async Task<bool> IsNameExistedAsync(string name)
        {
            return await _dbContext.BusinessProfiles
                .AsNoTracking()
                .AnyAsync(b => EF.Functions
                .Collate(b.Name, "SQL_Latin1_General_CP1_CI_AS")
                .Equals(name));
        }

        public async Task<bool> IsPhoneExistedAsync(string phone)
        {
            return await _dbContext.BusinessProfiles.AnyAsync(b => b.Phone.Equals(phone));
        }

        public async Task UpdateBusinessProfileAsync(BusinessProfile businessProfile)
        {
            await UpdateAsync(businessProfile);
        }

        public async Task<int> CountBusinessServiceByIdAsync(int id)
        {
            return await _dbContext.Services
                .AsNoTracking()
                //.GroupBy(s => new { s.Branch.BusinessId, s.Name })
                //.Select(s => new { s.Key.BusinessId, ServiceName = s.Key.Name })
                //.GroupBy(s => s.BusinessId)
                //.Select(s => new
                //{
                //    BusinessId = s.Key,
                //    DistinctService = s.ToList()
                //})
                .CountAsync();
        }

        public async Task<(IEnumerable<BusinessProfile>?, Pagination)> GetInvlaidateBusinessesProfilesAsync(GetInvalidateBusinessRequest getInvalidateBusinessRequest)
        {
            var businesses = _dbContext.BusinessProfiles.Where(b => b.Status.Equals(StatusConstants.PENDING)).AsQueryable();



            //Search
            if (!string.IsNullOrEmpty(getInvalidateBusinessRequest.SearchKey))
            {
                string searchKey = getInvalidateBusinessRequest.SearchKey;
                businesses = businesses.Where(b => EF.Functions.Like(b.Name, $"%{searchKey}%"));
            }

            //Order Sort
            if (getInvalidateBusinessRequest.SortBy != null)
            {
                businesses = getInvalidateBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Name)
                                : businesses.OrderBy(b => b.Name);
            }

            //Count Total Data
            int totalData = await businesses.AsNoTracking().CountAsync();

            //Paging
            int skipNum = (getInvalidateBusinessRequest.PageNum - 1) * getInvalidateBusinessRequest.PageSize;
            businesses = businesses.Skip(skipNum).Take(getInvalidateBusinessRequest.PageSize);

            //Paging Data Calulation
            var result = await businesses.ToListAsync();
            int totalPage = (int)Math.Ceiling((decimal)totalData / getInvalidateBusinessRequest.PageSize);

            var paging = new Pagination(totalData, getInvalidateBusinessRequest.PageSize, getInvalidateBusinessRequest.PageNum, totalPage);

            if (result == null || !result.Any())
            {
                return (null, paging);
            }

            return (result, paging);
        }

        public async Task<(IEnumerable<BusinessProfile>?, Pagination)> GetBusinessesByRankingAsync(GetBusinessRequest getBusinessRequest)
        {
            var businesses = _dbContext.BusinessProfiles.OrderBy(b => b.Rank).AsQueryable();

            //Search
            if (!string.IsNullOrEmpty(getBusinessRequest.SearchKey))
            {
                string searchKey = getBusinessRequest.SearchKey;
                businesses = businesses.Where(b => EF.Functions.Like(b.Name, $"%{searchKey}%"));
            }

            //Status Filter
            if (getBusinessRequest.Status != null)
            {
                //businesses = businesses.Where(b => b.Status.Equals(getBusinessRequest.Status));
                businesses = getBusinessRequest.Status switch
                {
                    BusinessStatus.ACTIVE => businesses.Where(b => b.Status.Equals(StatusConstants.ACTIVE)),
                    BusinessStatus.INACTIVE => businesses.Where(b => b.Status.Equals(StatusConstants.INACTIVE)),
                    BusinessStatus.SUSPENDED => businesses.Where(b => b.Status.Equals(StatusConstants.SUSPENDED)),
                    _ => businesses
                };
            }

            //Order Sort
            if (getBusinessRequest.SortBy != null)
            {
                businesses = getBusinessRequest.SortBy switch
                {
                    BusinessSortOption.NAME => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Name)
                                : businesses.OrderBy(b => b.Name),
                    BusinessSortOption.RATING => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Rating)
                                : businesses.OrderBy(b => b.Rating),
                    BusinessSortOption.RANK => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Rank)
                                : businesses.OrderBy(b => b.Rank),
                    BusinessSortOption.TOTAL => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.TotalOrder)
                                : businesses.OrderBy(b => b.TotalOrder),
                    BusinessSortOption.PENDING => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.PendingAmount)
                                : businesses.OrderBy(b => b.PendingAmount),
                    BusinessSortOption.PROCESSING => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.ProcessingAmount)
                                : businesses.OrderBy(b => b.ProcessingAmount),
                    BusinessSortOption.FINISHED => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.FinishedAmount)
                                : businesses.OrderBy(b => b.FinishedAmount),
                    BusinessSortOption.CANCEL => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.CanceledAmount)
                                : businesses.OrderBy(b => b.CanceledAmount),
                    BusinessSortOption.STATUS => getBusinessRequest.IsDecsending
                                ? businesses.OrderByDescending(b => b.Status)
                                : businesses.OrderBy(b => b.Status),
                    _ => businesses
                };
            }

            //Count Total Data
            int totalData = await businesses.AsNoTracking().CountAsync();

            //Paging
            int skipNum = (getBusinessRequest.PageNum - 1) * getBusinessRequest.PageSize;
            businesses = businesses.Skip(skipNum).Take(getBusinessRequest.PageSize);

            //Paging Data Calulation
            var result = await businesses.ToListAsync();
            int totalPage = (int)Math.Ceiling((decimal)totalData / getBusinessRequest.PageSize);

            var paging = new Pagination(totalData, getBusinessRequest.PageSize, getBusinessRequest.PageNum, totalPage);

            return (result, paging);
        }
    }
}