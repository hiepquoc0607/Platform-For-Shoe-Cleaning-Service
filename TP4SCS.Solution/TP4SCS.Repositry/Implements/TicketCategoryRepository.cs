using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class TicketCategoryRepository : GenericRepository<TicketCategory>, ITicketCategoryRepository
    {
        public TicketCategoryRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task CreateCategoryAsync(TicketCategory category)
        {
            await InsertAsync(category);
        }

        public async Task<IEnumerable<TicketCategory>?> GetCategoriesAsync()
        {
            return await _dbContext.TicketCategories.AsNoTracking().ToListAsync();
        }

        public async Task<TicketCategory?> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.TicketCategories.SingleOrDefaultAsync(c => c.Id == id && c.Status.Equals(StatusConstants.AVAILABLE));
        }

        public async Task<bool> IsNameExistedAsync(string name)
        {
            return await _dbContext.TicketCategories
                .AsNoTracking()
                .AnyAsync(c => EF.Functions.Collate(c.Name, "Vietnamese_CI_AS").Equals(name));
        }

        public async Task UpdateCategoryAsync(TicketCategory category)
        {
            await UpdateAsync(category);
        }
    }
}
