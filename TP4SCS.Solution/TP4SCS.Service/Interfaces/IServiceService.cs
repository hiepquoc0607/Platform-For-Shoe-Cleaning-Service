using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceService
    {
        Task<Service?> GetServiceByIdAsync(int id);
        Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc);
        Task<IEnumerable<Service>?> GetDiscountedServicesAsync();
        Task AddServiceAsync(ServiceCreateRequest service, int businessId);
        Task DeleteServiceAsync(int id);
        Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null);
        Task<decimal> GetServiceFinalPriceAsync(int serviceId);
        //Task<Service?> GetServicesByNameAndBranchIdAsync(string name, int branchId);
        //Task<(IEnumerable<Service> Services, int TotalCount)> GetServicesGroupByNameAsync(
        //    int? pageIndex = null,
        //    int? pageSize = null,
        //    OrderByEnum orderBy = OrderByEnum.IdAsc);
        //Task<IEnumerable<Service>?> GetServicesByBranchIdAsync(int branchId);
        //Task UpdateServiceStatusAsync(String status, int existingServiceId);
        Task UpdateServiceAsync(ServiceUpdateRequest serviceUpdateRequest, int existingServiceId);
    }
}
