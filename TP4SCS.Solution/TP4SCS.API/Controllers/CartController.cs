using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Cart;
using TP4SCS.Library.Models.Response.Cart;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/carts")]
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
        [Route("cart/{userId}")]
        public async Task<IActionResult> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound($"Cart for user ID {userId} not found.");
            }
            return Ok(new ResponseObject<CartResponse>("Fetch success", _mapper.Map<CartResponse>(cart)));
        }
        [HttpGet]
        [Route("total/{cartId}")]
        public async Task<IActionResult> GetCartTotal(int cartId)
        {
            var total = await _cartService.GetCartTotalAsync(cartId);
            return Ok(new { Total = total });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCart(int userId)
        {
            var cart = await _cartService.CreateCartAsync(userId);
            return CreatedAtAction(nameof(GetCartByUserIdAsync), new { userId = userId }, cart);
        }
        [HttpPut]
        [Route("{existingCartId}")]
        public async Task<IActionResult> UpdateCart(int existingCartId, [FromBody] CartUpdateRequest cartUpdateRequest)
        {
            var cart = _mapper.Map<Cart>(cartUpdateRequest);
            try
            {
                await _cartService.UpdateCartAsync(cart, existingCartId);
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
        [Route("clear/{cartId}")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            await _cartService.ClearCartAsync(cartId);
            return Ok();
        }
    }
}
