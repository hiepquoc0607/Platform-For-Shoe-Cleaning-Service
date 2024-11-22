using TP4SCS.Library.Models.Request.Payment;
using TP4SCS.Library.Models.Request.SubscriptionPack;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.SubcriptionPack;

namespace TP4SCS.Services.Interfaces
{
    public interface ISubscriptionPackService
    {
        Task<ApiResponse<IEnumerable<SubscriptionPackResponse>?>> GetPacksAsync();

        Task<ApiResponse<SubscriptionPackResponse?>> GetPackByIdAsync(int id);

        Task<ApiResponse<SubscriptionPackResponse>> CreatePackAsync(SubscriptionPackRequest subscriptionPackRequest);

        Task<ApiResponse<SubscriptionPackResponse>> UpdatePackAsync(int id, SubscriptionPackRequest subscriptionPackRequest);

        Task<ApiResponse<SubscriptionPackResponse>> DeletePackAsync(int id);
    }
}
