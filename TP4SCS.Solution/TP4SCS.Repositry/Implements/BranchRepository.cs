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
            return await _dbContext.BusinessBranches.Where(b => b.BusinessId == id).AsNoTracking().CountAsync();
        }

        public async Task CreateBranchAsync(BusinessBranch businessBranch)
        {
            await InsertAsync(businessBranch);
        }

        public async Task<BusinessBranch?> GetBranchByIdAsync(int id)
        {
            return await _dbContext.BusinessBranches.Where(b => b.Id == id).FirstOrDefaultAsync();
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
