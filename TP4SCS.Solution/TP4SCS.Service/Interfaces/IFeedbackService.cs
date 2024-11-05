using TP4SCS.Library.Models.Data;

namespace TP4SCS.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task AddFeedbacksAsync(Feedback feedback);
    }
}
