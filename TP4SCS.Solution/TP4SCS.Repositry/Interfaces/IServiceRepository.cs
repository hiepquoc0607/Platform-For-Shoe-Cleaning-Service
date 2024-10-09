using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetServices(string? keyword = null,
        int pageIndex = 1,
        int pageSize = 5,
        string orderBy = "Name");
        Task<Service> GetServiceById(int id);
        Task AddService(Service service);
        Task UpdateService(Service service);
        Task DeleteService(int id);
    }
}
