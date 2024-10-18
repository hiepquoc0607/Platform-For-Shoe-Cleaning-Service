using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
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
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/user/{id}/cart")]
        public async Task<IActionResult> GetCartByUserIdAsync(int id)
        {
            var cart = await _cartService.GetCartByUserIdAsync(id);
            if (cart == null)
            {
                return NotFound($"Giỏ hàng cho người dùng ID {id} không tìm thấy.");
            }
            return Ok(new ResponseObject<CartResponse>("Fetch success", _mapper.Map<CartResponse>(cart)));
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
        [HttpPut]
        [Route("api/carts/{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartUpdateRequest cartUpdateRequest)
        {
            var cart = _mapper.Map<Cart>(cartUpdateRequest);
            try
            {
                await _cartService.UpdateCartAsync(cart, id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("api/carts/{id}/clear")]
        public async Task<IActionResult> ClearCart(int id)
        {
            await _cartService.ClearCartAsync(id);
            return Ok();
        }
    }
}
