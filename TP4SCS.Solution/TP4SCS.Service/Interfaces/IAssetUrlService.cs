using Microsoft.AspNetCore.Http;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.AssetUrl;
using TP4SCS.Library.Models.Response.AssetUrl;

namespace TP4SCS.Services.Interfaces
{
    public interface IAssetUrlService
    {
        Task<List<AssetUrl>> AddAssestUrlsAsync(
            List<AssetUrlRequest> assetUrlRequests,
            int? businessId = null,
            int? feedbackId = null,
            int? serviceId = null);
        Task DeleteAssetUrlAsync(int assetUrlId);
        Task UpdateAssetUrlsAsync(List<AssetUrl> assetUrls);
    }
}
