using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Order;
using TP4SCS.Services.Interfaces;
using Mapster;
using TP4SCS.Library.Utils.StaticClass;

namespace TP4SCS.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public OrderController(IOrderService orderService, IEmailService emailService)
        {
            _orderService = orderService;
            _emailService = emailService;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync(
            [FromQuery] string? status = null,
            [FromQuery] int? pageIndex = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            var orders = await _orderService.GetOrdersAsync(status, pageIndex, pageSize, orderBy);
            var response = orders?.Adapt<IEnumerable<OrderResponse>>(); // Ánh xạ sang OrderResponse
            return Ok(response);
        }

        // GET: api/Order/account/{accountId}
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetOrdersByAccountIdAsync(
            int accountId,
            [FromQuery] string? status = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            var orders = await _orderService.GetOrdersByAccountIdAsync(accountId, status, orderBy);
            var response = orders?.Adapt<IEnumerable<OrderResponse>>();
            return Ok(response);
        }

        // GET: api/Order/branch/{branchId}
        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetOrdersByBranchIdAsync(
            int branchId,
            [FromQuery] string? status = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            var orders = await _orderService.GetOrdersByBranchIdAsync(branchId, status, orderBy);
            var response = orders?.Adapt<IEnumerable<OrderResponse>>();
            return Ok(response);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                await _emailService.SendEmailAsync(toEmail, subject, body);
                return Ok("Email đã được gửi thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi gửi email: {ex.Message}");
            }
        }
        [HttpPost("id")]
        public async Task<IActionResult> Approved(int id, string toEmail)
        {
            try
            {
                // Cập nhật trạng thái đơn hàng
                await _orderService.ApprovedOrder(id);

                // Thiết lập subject và body cho email
                string subject = "Đơn hàng đã được duyệt!";
                string body = $"Chào bạn,\n\n" +
                              $"Đơn hàng #{id} của bạn đã được duyệt thành công.\n" +
                              "Cảm ơn bạn đã chọn dịch vụ của chúng tôi.\n\n" +
                              "Trân trọng,\n" +
                              "Đội ngũ hỗ trợ khách hàng";

                // Gửi email
                await _emailService.SendEmailAsync(toEmail, subject, body);

                // Trả về thông báo thành công dưới dạng JSON
                return Ok(new { message = "Đơn hàng đã được duyệt thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Lỗi khi gửi email: {ex.Message}" });
            }
        }

    }
}