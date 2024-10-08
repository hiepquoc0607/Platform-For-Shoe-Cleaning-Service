using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request;
using TP4SCS.Library.Models.Response;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceService
    {
        Task<Service> GetServiceById(int id);
        Task<IEnumerable<Service>> GetServices(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name");
        Task AddService(ServiceRequest service);
        Task UpdateService(ServiceUpdateRequest service, int existingServiceId);
        Task DeleteService(int id);
    }
}
