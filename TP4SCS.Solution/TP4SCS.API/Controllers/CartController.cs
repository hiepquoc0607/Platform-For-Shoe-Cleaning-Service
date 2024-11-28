using Mapster;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Cart;
using TP4SCS.Library.Models.Response.Cart;
using TP4SCS.Library.Models.Response.CartItem;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Material;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Services.Implements;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IServiceService _serviceService;
        private readonly IMaterialService _materialService;
        private readonly HttpClient _httpClient;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartService cartService, IServiceService serviceService, IMaterialService materialService, ICartItemService cartItemService, IHttpClientFactory httpClientFactory)
        {
            _cartService = cartService;
            _serviceService = serviceService;
            _httpClient = httpClientFactory.CreateClient();
            _materialService = materialService;
            _cartItemService = cartItemService;
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
                List<CartItemResponse> cartItemsResponse = new List<CartItemResponse>();
                if (cart.CartItems != null && cart.CartItems.Any())
                {
                    foreach (var cartItem in cart.CartItems)
                    {
                        var item = await _cartItemService.GetCartItemByIdAsync(cartItem.Id);
                        if (item == null)
                        {
                            return NotFound(new ResponseObject<CartItemResponse>($"Mục có ID {id} không tìm thấy.", null));
                        }
                        var service = await _serviceService.GetServiceByIdAsync(item.ServiceId!.Value);
                        CartItemResponse cartItemResponse = new CartItemResponse
                        {
                            Id = item.Id,
                            BranchId = item.BranchId,
                            ServiceId = item.ServiceId!.Value,
                            ServiceName = service!.Name,
                            ServiceStatus = service!.BranchServices.SingleOrDefault(bs => bs.BranchId == item.BranchId)!.Status,
                            Price = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId!.Value),
                            CartId = cart.Id

                        };
                        List<MaterialResponseV2> materialResponseV2s = new List<MaterialResponseV2>();

                        if (!string.IsNullOrEmpty(item.MaterialIds))
                        {
                            List<int> materialIds = Util.ConvertStringToList(item.MaterialIds);
                            foreach (var materialId in materialIds)
                            {
                                var material = await _materialService.GetMaterialByIdAsync(materialId);
                                materialResponseV2s.Add(new MaterialResponseV2
                                {
                                    Id = materialId,
                                    Name = material!.Name,
                                    Status = material!.BranchMaterials.SingleOrDefault(ms => ms.BranchId == item.BranchId)!.Status,
                                    Price = material!.Price
                                });
                            }
                        }
                        cartItemsResponse.Add(cartItemResponse);
                        cartItemResponse.Materials = materialResponseV2s;
                    }
                    cartResponse.CartItems = cartItemsResponse;
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
        public async Task<IActionResult> CheckoutForCartItemAsync([FromBody] CheckoutCartRequest request)
        {
            if (request == null)
            {
                return BadRequest("Yêu cầu không hợp lệ. Vui lòng cung cấp sản phẩm trong giỏ hàng.");
            }

            try
            {
                await _cartService.CheckoutForCartAsync(_httpClient, request);
                return Ok("Thanh toán thành công.");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new ResponseObject<string>(ex.Message));
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
                var fee = await _cartService.GetFeeShip(_httpClient, addressId, branchId, quantity);
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
