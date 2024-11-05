using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface IFeedbackRepository
    {
        Task AddFeedbacksAsync(Feedback feedback);
    }
}
