using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Order;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Order;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IEmailService emailService, IMapper mapper)
        {
            _orderService = orderService;
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync(
            [FromQuery] string? status = null,
            [FromQuery] int? pageIndex = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync(status, pageIndex, pageSize, orderBy);
                var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
                return Ok(new ResponseObject<IEnumerable<OrderResponse>>("Lấy danh sách đơn hàng thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpGet("accounts/{accountId}")]
        public async Task<IActionResult> GetOrdersByAccountIdAsync(
            int accountId,
            [FromQuery] string? status = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            try
            {
                var orders = await _orderService.GetOrdersByAccountIdAsync(accountId, status, orderBy);
                var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
                return Ok(new ResponseObject<IEnumerable<OrderResponse>>("Lấy đơn hàng theo tài khoản thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            try
            {
                var orders = await _orderService.GetOrderByOrderId(id);
                var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
                return Ok(new ResponseObject<IEnumerable<OrderResponse>>("Lấy danh sách đơn hàng theo id thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }
        [HttpGet("branches/{id}")]
        public async Task<IActionResult> GetOrdersByBranchIdAsync(
            int id,
            [FromQuery] string? status = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            try
            {
                var orders = await _orderService.GetOrdersByBranchIdAsync(id, status, orderBy);
                var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
                return Ok(new ResponseObject<IEnumerable<OrderResponse>>("Lấy danh sách đơn hàng theo chi nhánh thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpGet("businesses/{id}")]
        public async Task<IActionResult> GetOrdersByBusinessIdAsync(
            int id,
            [FromQuery] string? status = null,
            [FromQuery] OrderedOrderByEnum orderBy = OrderedOrderByEnum.CreateDateAsc)
        {
            try
            {
                var orders = await _orderService.GetOrdersByBusinessIdAsync(id, status, orderBy);
                var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
                return Ok(new ResponseObject<IEnumerable<OrderResponse>>("Lấy danh sách đơn hàng theo doanh nghiệp thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpPut("{id}/approved")]
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

                // Trả về thông báo thành công dưới dạng ResponseObject
                return Ok(new ResponseObject<string>("Đơn hàng đã được duyệt thành công!", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Lỗi khi gửi email: {ex.Message}"));
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(int id, UpdateOrderRequest request)
        {
            try
            {
                await _orderService.UpdateOrderAsync(id, request);
                return Ok(new ResponseObject<string>("Đơn hàng đã được cập nhật thành công!", null));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ResponseObject<string>($"Không tìm thấy đơn hàng với ID: {id}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Lỗi khi cập nhật: {ex.Message}", null));
            }
        }
    }
}
