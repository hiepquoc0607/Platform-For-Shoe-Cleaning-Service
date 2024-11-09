using Mapster;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Request.Cart;
using TP4SCS.Library.Models.Response.Cart;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IServiceService _serviceService;
        private readonly HttpClient _httpClient;
        public CartController(ICartService cartService, IServiceService serviceService, IHttpClientFactory httpClientFactory)
        {
            _cartService = cartService;
            _serviceService = serviceService;
            _httpClient = httpClientFactory.CreateClient();
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
                    await _cartService.CreateCartAsync(id);
                    cart = await _cartService.GetCartByUserIdAsync(id);
                }

                var cartResponse = cart.Adapt<CartResponse>();
                cartResponse.TotalPrice = await _cartService.GetCartTotalAsync(cart!.Id);

                if (cartResponse.CartItems != null && cartResponse.CartItems.Any())
                {
                    foreach (var cartItem in cartResponse.CartItems)
                    {
                        var service = await _serviceService.GetServiceByIdAsync(cartItem.ServiceId);
                        cartItem.Price = await _serviceService.GetServiceFinalPriceAsync(cartItem.ServiceId);
                        cartItem.ServiceName = service!.Name;
                        cartItem.ServiceStatus = service!.BranchServices.SingleOrDefault(bs => bs.BranchId == cartItem.BranchId)!.Status;
                    }
                    var groupedCartItems = cartResponse.CartItems
                        .GroupBy(item => item.BranchId)
                        .Select(group => new
                        {
                            BranchId = group.Key,
                            Items = group.ToList()
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


        //[HttpGet]
        //[Route("api/carts/{id}/total")]
        //public async Task<IActionResult> GetCartTotal(int id)
        //{
        //    var total = await _cartService.GetCartTotalAsync(id);
        //    return Ok(new { Total = total });
        //}
        [HttpPost("api/carts/cart/checkout")]
        public async Task<IActionResult> CheckoutForCartItemAsync([FromBody] CheckoutForCartItemRequest request)
        {
            if (request == null)
            {
                return BadRequest("Yêu cầu không hợp lệ. Vui lòng cung cấp sản phẩm trong giỏ hàng.");
            }

            try
            {
                await _cartService.CheckoutForCartItemAsync(_httpClient, request);
                return Ok("Thanh toán thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }
        [HttpPost("api/services/service/checkout")]
        public async Task<IActionResult> CheckoutForServiceAsync([FromBody] CheckoutForServiceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Yêu cầu không hợp lệ. Vui lòng cung cấp sản phẩm trong giỏ hàng.");
            }

            try
            {
                await _cartService.CheckoutForServiceAsync(_httpClient, request);
                return Ok("Thanh toán thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }
        [HttpGet("api/carts/cart/checkout/feeship")]
        public async Task<IActionResult> Feeship(int addressId, int branchId, int quantity)
        {

            try
            {
                var fee = await _cartService.GetFeeShip(_httpClient, addressId,branchId,quantity);
                return Ok(fee);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        //[HttpPost]
        //[Route("api/carts")]
        //public async Task<IActionResult> CreateCart(int userId)
        //{
        //    var cart = await _cartService.CreateCartAsync(userId);
        //    return CreatedAtAction(nameof(GetCartByUserIdAsync), new { userId = userId }, cart);
        //}
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
