using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Service;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceService
    {
        Task<Service?> GetServiceByIdAsync(int id);
        Task<IEnumerable<Service>?> GetServicesAsync(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name");
        Task AddServiceAsync(ServiceRequest service);
        Task UpdateServiceAsync(ServiceUpdateRequest service, int existingServiceId);
        Task DeleteServiceAsync(int id);
    }
}
