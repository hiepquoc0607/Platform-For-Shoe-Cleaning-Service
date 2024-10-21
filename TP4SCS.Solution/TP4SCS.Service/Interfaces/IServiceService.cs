using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceService
    {
        Task<Service?> GetServiceByIdAsync(int id);
        Task<IEnumerable<Service>?> GetServicesAsync(string? keyword = null, string? status = null,
            int pageIndex = 1, int pageSize = 5, OrderByEnum orderBy = OrderByEnum.IdAsc);
        Task<IEnumerable<Service>?> GetDiscountedServicesAsync();
        Task AddServiceAsync(ServiceCreateRequest service);
        Task UpdateServiceAsync(ServiceUpdateRequest service, int existingServiceId);
        Task DeleteServiceAsync(int id);
        Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null);
    }
}
