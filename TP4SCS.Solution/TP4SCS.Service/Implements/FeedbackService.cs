using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<IEnumerable<Feedback>?> GetFeedbackByServiceId(int serviceId)
        {
            return await _feedbackRepository.GetFeedbacksByServiceIdAsync(serviceId);
        }
        public async Task AddFeedbacksAsync(Feedback feedback)
        {
            if (feedback.Rating < 0 || feedback.Rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(feedback.Rating), "Rating phải nằm trong khoảng từ 0 đến 5.");
            }

            if (feedback.Content != null && feedback.Content.Length > 500)
            {
                throw new ArgumentException("Nội dung feedback không được vượt quá 500 ký tự.", nameof(feedback.Content));
            }
            if (string.IsNullOrWhiteSpace(feedback.Status))
            {
                throw new ArgumentException("Trạng thái của feedback không được để trống.", nameof(feedback.Status));
            }
            feedback.CreatedTime = DateTime.UtcNow;
            await _feedbackRepository.AddFeedbacksAsync(feedback);
        }
        public async Task DeleteFeedbackAsync(int id)
        {
            await _feedbackRepository.DeleteFeedbackAsync(id);
        }
        public async Task UpdateFeedbackAsync(Feedback feedback, int existingFeedbackId)
        {
            var existingFeedback = await _feedbackRepository.GetFeedbackByidAsync(existingFeedbackId);
            if(existingFeedback == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đánh giá với ID: {existingFeedbackId}.");
            }
            existingFeedback.Status = feedback.Status;
            existingFeedback.Content = feedback.Status;
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
        }
    }
}
