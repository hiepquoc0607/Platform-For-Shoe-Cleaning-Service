using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class BranchRepository : GenericRepository<BusinessBranch>, IBranchRepository
    {
        public BranchRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountBranchDataByBusinessIdAsync(int id)
        {
            return await _dbContext.BusinessBranches.AsNoTracking().CountAsync(b => b.BusinessId == id);
        }

        public async Task CreateBranchAsync(BusinessBranch businessBranch)
        {
            await InsertAsync(businessBranch);
        }

        public async Task<BusinessBranch?> GetBranchByIdAsync(int id)
        {
            return await _dbContext.BusinessBranches.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BusinessBranch>?> GetBranchesByBusinessIdAsync(int id)
        {
            return await _dbContext.BusinessBranches.Where(b => b.BusinessId == id).ToListAsync();
        }

        public async Task<int[]?> GetBranchesIdByOwnerIdAsync(int id)
        {
            return await _dbContext.BusinessProfiles
                .AsNoTracking()
                .Where(p => p.OwnerId == id)
                .SelectMany(p => p.BusinessBranches)
                .Select(b => b.Id)
                .ToArrayAsync();
        }

        public async Task<int?> GetBranchIdByEmployeeIdAsync(int id)
        {
            return await _dbContext.BusinessBranches
                .AsNoTracking()
                .Where(b => EF.Functions.Like(b.EmployeeIds, $"%{id.ToString()}%"))
                .Select(b => b.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetBranchMaxIdAsync()
        {
            return await _dbContext.BusinessBranches.AsNoTracking().MaxAsync(b => b.Id);
        }

        public async Task UpdateBranchAsync(BusinessBranch businessBranch)
        {
            await UpdateAsync(businessBranch);
        }
    }
}
