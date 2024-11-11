using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IFeedbackRepository
    {
        Task AddFeedbacksAsync(Feedback feedback);
        Task<IEnumerable<Feedback>?> GetFeedbacksAsync(
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnumV2 orderBy = OrderByEnumV2.CreateDes);
        Task<IEnumerable<Feedback>?> GetFeedbacksByServiceIdAsync(
            int serviceId,
            string? status = null,
            int? pageIndex = null,
            int? pageSize = null,
            OrderByEnum orderBy = OrderByEnum.IdAsc);
        Task DeleteFeedbackAsync(int id);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task<Feedback?> GetFeedbackByidAsync(int id);
        Task<IEnumerable<Feedback>?> GetFeedbacksByAccountIdAsync(
           int accountId,
           string? status = null,
           int? pageIndex = null,
           int? pageSize = null,
           OrderByEnum orderBy = OrderByEnum.IdAsc);
    }
}
