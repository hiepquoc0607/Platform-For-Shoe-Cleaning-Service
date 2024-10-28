using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Response.Cart;
using TP4SCS.Library.Models.Response.CartItem;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IServiceService _serviceService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IServiceService serviceService, IMapper mapper)
        {
            _cartService = cartService;
            _serviceService = serviceService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/user/{id}/cart")]
        public async Task<IActionResult> GetCartByUserIdAsync(int id)
        {
            try
            {
                var cart = await _cartService.GetCartByUserIdAsync(id);
                if (cart == null)
                {
                    return NotFound($"Giỏ hàng cho người dùng ID {id} không tìm thấy.");
                }

                var cartResponse = _mapper.Map<CartResponse>(cart);
                cartResponse.TotalPrice = await _cartService.GetCartTotalAsync(cart.Id);

                if (cartResponse.CartItems != null && cartResponse.CartItems.Any())
                {
                    // Lấy giá cuối cùng cho từng dịch vụ trong giỏ hàng
                    foreach (var cartItem in cartResponse.CartItems)
                    {
                        cartItem.Price = await _serviceService.GetServiceFinalPriceAsync(cartItem.ServiceId);
                    }

                    // Lấy BranchId cho từng cartItem trước khi group
                    var itemsWithBranchId = new List<(CartItemResponse Item, int? BranchId)>();
                    foreach (var cartItem in cartResponse.CartItems)
                    {
                        var service = await _serviceService.GetServiceByIdAsync(cartItem.ServiceId);
                        itemsWithBranchId.Add((cartItem, service?.BranchId));
                    }

                    // Group theo BranchId đã lấy
                    var groupedCartItems = itemsWithBranchId
                        .GroupBy(x => x.BranchId)
                        .Select(group => new
                        {
                            BranchId = group.Key,
                            Items = group.Select(x => x.Item).ToList()
                        })
                        .ToList();

                    return Ok(new ResponseObject<IEnumerable<dynamic>>("Fetch success", groupedCartItems));
                }

                return Ok(new ResponseObject<CartResponse>("Fetch success", cartResponse));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Lỗi: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi trong quá trình xử lý yêu cầu: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("api/carts/{id}/total")]
        public async Task<IActionResult> GetCartTotal(int id)
        {
            var total = await _cartService.GetCartTotalAsync(id);
            return Ok(new { Total = total });
        }
        [HttpPost]
        [Route("api/carts")]
        public async Task<IActionResult> CreateCart(int userId)
        {
            var cart = await _cartService.CreateCartAsync(userId);
            return CreatedAtAction(nameof(GetCartByUserIdAsync), new { userId = userId }, cart);
        }
        //[HttpPut]
        //[Route("api/carts/{id}")]
        //public async Task<IActionResult> UpdateCart(int id, [FromBody] CartUpdateRequest cartUpdateRequest)
        //{
        //    var cart = _mapper.Map<Cart>(cartUpdateRequest);
        //    try
        //    {
        //        await _cartService.UpdateCartAsync(cart, id);
        //        return Ok();
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpDelete]
        [Route("api/carts/{id}/clear")]
        public async Task<IActionResult> ClearCart(int id)
        {
            await _cartService.ClearCartAsync(id);
            return Ok();
        }
    }
}
