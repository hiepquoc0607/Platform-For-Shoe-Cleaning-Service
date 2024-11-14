using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class SubscriptionRepository : GenericRepository<SubscriptionPack>, ISubscriptionRepository
    {
        public SubscriptionRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task CreatePackAsync(SubscriptionPack subscriptionPack)
        {
            await InsertAsync(subscriptionPack);
        }

        public async Task<SubscriptionPack?> GetPackByIdAsync(int id)
        {
            return await _dbContext.SubscriptionPacks.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetPackMaxIdAsync()
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().MaxAsync(p => p.Id);
        }

        public async Task<IEnumerable<SubscriptionPack>?> GetPacksAsync()
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsPackNameExistedAsync(string name)
        {
            return await _dbContext.SubscriptionPacks.AsNoTracking().AnyAsync(p => p.Name.Equals(name));
        }

        public async Task UpdatePackAsync(SubscriptionPack subscriptionPack)
        {
            await UpdateAsync(subscriptionPack);
        }
    }
}
