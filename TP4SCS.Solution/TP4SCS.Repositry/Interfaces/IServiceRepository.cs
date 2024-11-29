using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>?> GetServicesAsync(string? keyword = null, string? status = null,
        int? pageIndex = null,
        int? pageSize = null,
        OrderByEnum orderBy = OrderByEnum.IdAsc);

        Task<Service?> GetServiceByIdAsync(int id);

        Task AddServicesAsync(List<Service> services);

        Task AddServiceAsync(int[] branchIds, int businessId, Service service);

        Task UpdateServiceAsync(Service service, int[] branchIds);
        Task UpdateServiceAsync(Service service);

        Task DeleteServiceAsync(int id);

        Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null);

        Task<IEnumerable<Service>> GetServicesAsync(string? keyword = null, string? status = null);
    }
}
