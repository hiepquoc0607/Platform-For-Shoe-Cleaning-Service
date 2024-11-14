using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface ISubscriptionRepository : IGenericRepository<SubscriptionPack>
    {
        Task<IEnumerable<SubscriptionPack>?> GetPacksAsync();

        Task<SubscriptionPack?> GetPackByIdAsync(int id);

        Task<int> GetPackMaxIdAsync();

        Task<bool> IsPackNameExistedAsync(string name);

        Task CreatePackAsync(SubscriptionPack subscriptionPack);

        Task UpdatePackAsync(SubscriptionPack subscriptionPack);
    }
}
