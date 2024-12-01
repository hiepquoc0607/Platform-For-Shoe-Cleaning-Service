using TP4SCS.Library.Models.Request.SubscriptionPack;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.SubcriptionPack;

namespace TP4SCS.Services.Interfaces
{
    public interface IPlatformPackService
    {
        Task<ApiResponse<IEnumerable<PlatformPackResponse>?>> GetPacksAsync();

        Task<ApiResponse<PlatformPackResponse?>> GetPackByIdAsync(int id);

        Task<ApiResponse<PlatformPackResponse>> CreatePackAsync(PlatformPackRequest subscriptionPackRequest);

        Task<ApiResponse<PlatformPackResponse>> UpdatePackAsync(int id, PlatformPackRequest subscriptionPackRequest);

        Task<ApiResponse<PlatformPackResponse>> DeletePackAsync(int id);
    }
}
