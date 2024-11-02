using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Order;
using TP4SCS.Services.Interfaces;
using Mapster;

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
    }
}