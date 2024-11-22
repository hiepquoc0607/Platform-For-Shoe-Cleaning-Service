using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.CartItem;
using TP4SCS.Library.Models.Response.CartItem;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly IServiceService _serviceService;
        private readonly IMapper _mapper;

        public CartItemController(ICartItemService cartItemService, IServiceService serviceService, IMapper mapper)
        {
            _cartItemService = cartItemService;
            _serviceService = serviceService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/cart/{id}/cartitems")]
        public async Task<IActionResult> GetCartItems(int id)
        {
            var items = await _cartItemService.GetCartItemsAsync(id);

            var itemsResponse = items.Adapt<List<CartItemResponse>>();

            foreach (var item in itemsResponse)
            {
                var service = await _serviceService.GetServiceByIdAsync(item.ServiceId);
                item.ServiceName = service!.Name;
                item.ServiceStatus = service!.BranchServices.SingleOrDefault(bs => bs.BranchId == item.BranchId)!.Status;
            }
            return Ok(new ResponseObject<List<CartItemResponse>>("Cart items retrieved successfully", itemsResponse));
        }

        [HttpGet]
        [Route("api/cartitems/{id}")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            var item = await _cartItemService.GetCartItemByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ResponseObject<CartItemResponse>($"Mục có ID {id} không tìm thấy.", null));
            }

            var itemResponse = item.Adapt<CartItemResponse>();


            var service = await _serviceService.GetServiceByIdAsync(itemResponse.ServiceId);
            itemResponse.ServiceName = service!.Name;
            itemResponse.ServiceStatus = service!.BranchServices.SingleOrDefault(bs => bs.BranchId == item.BranchId)!.Status;

            return Ok(new ResponseObject<CartItemResponse>("Cart item retrieved successfully", itemResponse));
        }

        //[HttpGet]
        //[Route("api/cartitems/total")]
        //public async Task<IActionResult> CalculateCartItemsTotal([FromQuery] List<int> itemIds)
        //{
        //    try
        //    {
        //        decimal? total = await _cartItemService.CalculateCartItemsTotal(itemIds);

        //        return Ok(new ResponseObject<decimal?>("Tính toán thành công", total));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
        //    }
        //}
        [HttpPost]
        [Route("api/cartitems")]
        public async Task<IActionResult> AddItemToCart(int userId, [FromBody] CartItemCreateRequest request)
        {
            var item = request.Adapt<CartItem>();
            try
            {
                await _cartItemService.AddItemToCartAsync(userId, item);

                return Ok(new ResponseObject<string>("Add to cart success"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("api/cartitems/batch")]
        public async Task<IActionResult> AddItemsToCart(int userId, [FromBody] IEnumerable<CartItemCreateRequest> request)
        {
            if (request == null || !request.Any())
            {
                return BadRequest("Danh sách sản phẩm không được rỗng.");
            }

            try
            {
                // Map danh sách yêu cầu thành CartItem
                var items = _mapper.Map<IEnumerable<CartItem>>(request);

                // Gọi service để thêm danh sách vào giỏ hàng
                await _cartItemService.AddItemsToCartAsync(userId, items.ToList());

                return Ok(new ResponseObject<string>("Thêm các sản phẩm vào giỏ hàng thành công."));
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi do logic không hợp lệ
                return BadRequest(new ResponseObject<string>(ex.Message));
            }
            catch (Exception ex)
            {
                // Lỗi không mong muốn
                return StatusCode(500, new ResponseObject<string>($"Lỗi hệ thống: {ex.Message}"));
            }
        }

        [HttpPut]
        [Route("api/cartitems/{id}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int id, [FromBody] int newQuantity)
        {
            try
            {
                await _cartItemService.UpdateCartItemQuantityAsync(id, newQuantity);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/cartitems")]
        public async Task<IActionResult> RemoveItemsFromCart([FromBody] int[] itemIds)
        {
            try
            {
                if (itemIds == null || itemIds.Length == 0)
                {
                    return BadRequest("Danh sách ID của các mục không được để trống.");
                }

                await _cartItemService.RemoveItemsFromCartAsync(itemIds);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi máy chủ nội bộ: " + ex.Message);
            }
        }


    }
}
