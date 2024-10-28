using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface IBusinessRepository : IGenericRepository<BusinessProfile>
    {
        Task<int?> GetBusinessIdByOwnerIdAsync(int id);
    }
}
