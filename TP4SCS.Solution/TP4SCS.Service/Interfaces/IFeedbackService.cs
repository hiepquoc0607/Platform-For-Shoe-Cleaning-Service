using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>?> GetFeedbacks(string? status, OrderByEnum order);

        Task AddFeedbacksAsync(HttpClient httpClient, Feedback feedback);

        Task<IEnumerable<Feedback>?> GetFeedbackByServiceId(int serviceId);

        Task DeleteFeedbackAsync(int id);

        Task UpdateFeedbackAsync(bool? isValidAsset, bool? IsValidContent, string? status, int existingFeedbackId);

        Task<IEnumerable<Feedback>?> GetFeedbackByAccountId(int accountId);
    }
}
