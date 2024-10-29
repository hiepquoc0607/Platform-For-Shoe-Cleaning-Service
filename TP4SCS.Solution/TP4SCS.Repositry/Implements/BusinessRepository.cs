using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class BusinessRepository : GenericRepository<BusinessProfile>, IBusinessRepository
    {
        public BusinessRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<int?> GetBusinessIdByOwnerIdAsync(int id)
        {
            return await _dbContext.BusinessProfiles.AsNoTracking()
                .Where(p => p.OwnerId == id)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }
    }
}
