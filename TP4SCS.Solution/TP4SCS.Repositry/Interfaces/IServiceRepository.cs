using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>?> GetServicesAsync(string? keyword = null,string? status = null,
        int? pageIndex = null,
        int? pageSize = null,
        OrderByEnum orderBy = OrderByEnum.IdAsc);
        Task<Service?> GetServiceByIdAsync(int id);
        Task AddServiceAsync(List<Service> services);
        Task UpdateServiceAsync(Service service);
        Task DeleteServiceAsync(int id);
        Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null);
        Task<IEnumerable<Service>> GetServicesAsync(string? keyword = null, string? status = null);
        Task<IEnumerable<Service>> GetServicesIncludeBranchAsync();
    }
}
