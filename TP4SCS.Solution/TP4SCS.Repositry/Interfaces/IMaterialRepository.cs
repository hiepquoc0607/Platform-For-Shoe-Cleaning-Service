using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IMaterialRepository
    {
        Task AddMaterialAsync(int serviceId, Material material);

        Task DeleteMaterialAsync(int id);

        Task<Material?> GetMaterialByIdAsync(int id);

        Task<IEnumerable<Material>?> GetMaterialsAsync(
            string? keyword = null,
            string? status = null,
            int pageIndex = 1,
            int pageSize = 5,
            OrderByEnum orderBy = OrderByEnum.IdAsc);

        Task<IEnumerable<Material>> GetMaterialsAsync(string? keyword = null, string? status = null);

        Task<int> GetTotalMaterialCountAsync(string? keyword = null, string? status = null);

        Task UpdateMaterialAsync(Material material);
    }
}
