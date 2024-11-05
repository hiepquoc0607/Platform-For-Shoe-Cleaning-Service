using Mapster;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.OrderDetail;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.OrderDetail;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        [Route("api/orderdetails/{id}")]
        public async Task<IActionResult> GetOrderDetailByIdAsync(int id)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
                if (orderDetail == null)
                {
                    return NotFound(new ResponseObject<OrderDetailResponse>($"Không tìm thấy chi tiết đơn hàng với ID {id}.", null));
                }

                var response = orderDetail.Adapt<OrderDetailResponse>();
                return Ok(new ResponseObject<OrderDetailResponse>("Lấy chi tiết đơn hàng thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpGet]
        [Route("api/order/{id}/orderdetails")]
        public async Task<IActionResult> GetOrderDetailsByOrderIdAsync(int id)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(id);
                if (orderDetails == null || !orderDetails.Any())
                {
                    return NotFound(new ResponseObject<IEnumerable<OrderDetailResponse>>("Không tìm thấy chi tiết đơn hàng cho đơn hàng này."));
                }

                var response = orderDetails.Adapt<IEnumerable<OrderDetailResponse>>();
                return Ok(new ResponseObject<IEnumerable<OrderDetailResponse>>("Lấy danh sách chi tiết đơn hàng thành công.", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpPost]
        [Route("api/orderdetails")]
        public async Task<IActionResult> AddOrderDetailsAsync([FromBody] OrderDetailCreateRequest request)
        {
            try
            {
                var orderDetail = request.Adapt<OrderDetail>();
                await _orderDetailService.AddOrderDetailAsync(orderDetail);
                return Ok(new ResponseObject<string>("Thêm chi tiết đơn hàng thành công."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseObject<string>($"Lỗi hợp lệ: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpPut]
        [Route("api/orderdetails/{id}")]
        public async Task<IActionResult> UpdateOrderDetailAsync(int id, [FromBody] OrderDetailRequest request)
        {
            try
            {
                await _orderDetailService.UpdateOrderDetailAsync(request, id);

                var updatedOrderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
                return Ok(new ResponseObject<string>("Cập nhật chi tiết đơn hàng thành công."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseObject<string>($"Lỗi hợp lệ: {ex.Message}"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpDelete]
        [Route("api/orderdetails/{id}")]
        public async Task<IActionResult> DeleteOrderDetailAsync(int id)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetailAsync(id);
                return Ok(new ResponseObject<string>("Xóa chi tiết đơn hàng thành công."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseObject<string>($"Lỗi hợp lệ: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<string>($"Lỗi: {ex.Message}"));
            }
        }
    }
}
