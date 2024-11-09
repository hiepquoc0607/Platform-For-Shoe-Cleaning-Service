using TP4SCS.Library.Models.Data;

namespace TP4SCS.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task AddFeedbacksAsync(Feedback feedback);
        Task<IEnumerable<Feedback>?> GetFeedbackByServiceId(int serviceId);
        Task DeleteFeedbackAsync(int id);
        Task UpdateFeedbackAsync(Feedback feedback, int existingFeedbackId);
        Task<IEnumerable<Feedback>?> GetFeedbackByAccountId(int accountId);
    }
}
