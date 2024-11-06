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

        public async Task CreateBusinessProfileAsync(BusinessProfile businessProfile, BusinessBranch businessBranch)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await InsertAsync(businessProfile);

                await _branchRepository.CreateBranchAsync(businessBranch);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
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
            return await _dbContext.BusinessProfiles
                .AnyAsync(b => EF.Functions
                .Collate(b.Name, "SQL_Latin1_General_CP1_CI_AI")
                .Contains(name));
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
    }
}
