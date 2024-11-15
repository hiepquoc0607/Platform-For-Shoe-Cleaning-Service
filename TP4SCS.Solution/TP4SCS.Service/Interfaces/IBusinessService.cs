using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Response.BusinessProfile;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IBusinessService
    {
        Task<ApiResponse<IEnumerable<BusinessResponse>?>> GetBusinessesProfilesAsync(GetBusinessRequest getBusinessRequest);

        Task<ApiResponse<IEnumerable<BusinessResponse>?>> GetBusinessesByRankAsync(GetBusinessRequest getBusinessRequest);

        Task<ApiResponse<IEnumerable<BusinessResponse>?>> GetInvalidateBusinessesProfilesAsync(GetInvalidateBusinessRequest getInvalidateBusinessRequest);

        Task<ApiResponse<BusinessResponse?>> GetBusinessProfileByIdAsync(int id);

        Task<bool> CheckOwnerOfBusiness(int ownerId, int businessId);

        Task<int?> GetBusinessIdByOwnerId(int id);

        Task<ApiResponse<BusinessResponse>> UpdateBusinessProfileAsync(int id, UpdateBusinessRequest updateBusinessRequest);

        Task<ApiResponse<BusinessResponse>> UpdateBusinessRankAsync(int id, UpdateBusinessRankRequest updateBusinessRankRequest);

        Task<ApiResponse<BusinessResponse>> UpdateBusinessStatisticAsync(int id, UpdateBusinessStatisticRequest updateBusinessStatisticRequest);

        Task<ApiResponse<BusinessResponse>> UpdateBusinessSubscriptionAsync(int id, UpdateBusinessSubcriptionRequest updateBusinessSubcriptionRequest);

        Task<ApiResponse<BusinessResponse>> UpdateBusinessStatusForAdminAsync(int id, UpdateBusinessStatusRequest updateBusinessStatusRequest);

        Task<ApiResponse<BusinessResponse>> ValidateBusinessAsync(int id, ValidateBusinessRequest validateBusinessRequest);
    }
}
