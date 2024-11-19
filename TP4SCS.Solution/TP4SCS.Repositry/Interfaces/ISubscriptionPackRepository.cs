using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface ISubscriptionPackRepository : IGenericRepository<SubscriptionPack>
    {
        Task<IEnumerable<SubscriptionPack>?> GetPacksAsync();

        Task<SubscriptionPack?> GetPackByIdAsync(int id);

        Task<decimal> GetPackPriceByPeriodAsync(int period);

        Task<int> GetPackMaxIdAsync();

        Task<int> CountPackAsync();

        Task<List<int>> GetPeriodArrayAsync();

        Task<bool> IsPackNameExistedAsync(string name);

        Task CreatePackAsync(SubscriptionPack subscriptionPack);

        Task UpdatePackAsync(SubscriptionPack subscriptionPack);
    }
}
