using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class SubscriptionPackRepository : GenericRepository<SubscriptionPack>, ISubscriptionPackRepository
    {
        public SubscriptionPackRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountPackAsync()
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().CountAsync();
        }

        public async Task CreatePackAsync(SubscriptionPack subscriptionPack)
        {
            await InsertAsync(subscriptionPack);
        }

        public async Task<SubscriptionPack?> GetPackByIdAsync(int id)
        {
            return await _dbContext.SubscriptionPacks.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<decimal> GetPackPriceByPeriodAsync(int period)
        {
            return await _dbContext.SubscriptionPacks.Where(p => p.Period == period).Select(p => p.Price).SingleOrDefaultAsync();
        }

        public async Task<int> GetPackMaxIdAsync()
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().Where(p => p.Period != 0).MaxAsync(p => p.Id);
        }

        public async Task<IEnumerable<SubscriptionPack>?> GetPacksAsync()
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().Where(p => p.Period != 0).ToListAsync();
        }

        public async Task<List<int>> GetPeriodArrayAsync()
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().Where(p => p.Period != 0).Select(p => p.Period).ToListAsync();
        }

        public async Task<bool> IsPackNameExistedAsync(string name)
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().AnyAsync(p => p.Name.Equals(name) && p.Period != 0);
        }

        public async Task UpdatePackAsync(SubscriptionPack subscriptionPack)
        {
            await UpdateAsync(subscriptionPack);
        }
    }
}
