using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IMaterialService
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

        Task<IEnumerable<Material>> GetAllMaterialsAsync(string? keyword = null, string? status = null);

        Task<int> GetTotalMaterialCountAsync(string? keyword = null, string? status = null);

        Task UpdateMaterialAsync(int id, Material material);
    }
}
