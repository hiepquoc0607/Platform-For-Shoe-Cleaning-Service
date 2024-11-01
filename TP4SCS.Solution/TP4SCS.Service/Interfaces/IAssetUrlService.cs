using Microsoft.AspNetCore.Http;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Response.AssetUrl;

namespace TP4SCS.Services.Interfaces
{
    public interface IAssetUrlService
    {
        Task<List<AssetUrl>> AddAssestUrlsAsync(
            List<FileResponse> files,
            int? businessId = null,
            int? feedbackId = null,
            int? serviceId = null);
        Task<FileResponse> UploadFileAsync(IFormFile file);
        Task<List<FileResponse>> UploadFilesAsync(List<IFormFile> files);
        Task DeleteImageAsync(string fileUrl);
        Task DeleteAssetUrlAsync(int assetUrlId);
        Task UpdateAssetUrlsAsync(List<AssetUrl> assetUrls);
    }
}
