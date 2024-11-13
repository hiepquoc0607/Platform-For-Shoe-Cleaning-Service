using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Service;

namespace TP4SCS.Services.Interfaces
{
    public interface IServiceService
    {
        Task<Service?> GetServiceByIdAsync(int id);

        Task<ApiResponse<IEnumerable<ServiceResponse>?>> GetServiceByBusinessIdAsync(GetBusinessServiceRequest getBusinessServiceRequest);

        Task<IEnumerable<Service>?> GetServicesAsync(
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc);

        Task<(IEnumerable<Service>?, int)> GetDiscountedServicesAsync(
            string? name = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null);

        Task AddServiceAsync(ServiceCreateRequest service, int businessId);

        Task DeleteServiceAsync(int id);

        Task<int> GetTotalServiceCountAsync(string? keyword = null, string? status = null);

        Task<decimal> GetServiceFinalPriceAsync(int serviceId);

        Task<(IEnumerable<Service>?, int)> GetServicesByBranchIdAsync(
            int branchId,
            string? keyword = null,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc);

        Task UpdateServiceAsync(ServiceUpdateRequest serviceUpdateRequest, int existingServiceId);

    }
}
