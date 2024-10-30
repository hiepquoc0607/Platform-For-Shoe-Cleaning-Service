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

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
    }
}