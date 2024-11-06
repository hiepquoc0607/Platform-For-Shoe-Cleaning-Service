using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Feedback;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Service;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public FeedbackController(IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequest feedbackRequest)
        {          
            try
            {
                var feedback = _mapper.Map<Feedback>(feedbackRequest);
                await _feedbackService.AddFeedbacksAsync(feedback);
                return Ok(new ResponseObject<string>("Tạo đánh giá thành công"));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseObject<string>($"Lỗi: {ex.Message}"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<string>($"Lỗi: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi không mong muốn: {ex.Message}"));
            }
        }
    }
}
