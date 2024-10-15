using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.CartItem;
using TP4SCS.Library.Models.Response.CartItem;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/cartitems")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;

        public CartItemController(ICartItemService cartItemService, IMapper mapper)
        {
            _cartItemService = cartItemService;
            _mapper = mapper;
        }

        [HttpGet("cart/{cartId}")]
        public async Task<IActionResult> GetCartItems(int cartId)
        {
            var items = await _cartItemService.GetCartItemsAsync(cartId);

            return Ok(new ResponseObject<List<CartItemResponse>>("Cart items retrieved successfully",
                _mapper.Map<List<CartItemResponse>>(items)));
        }


        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetCartItemById(int itemId)
        {
            var item = await _cartItemService.GetCartItemByIdAsync(itemId);
            if (item == null)
            {
                return Ok(new ResponseObject<CartItemResponse>($"Item with ID {itemId} not found.", null));
            }
            return Ok(new ResponseObject<CartItemResponse>("Cart item retrieved successfully", _mapper.Map<CartItemResponse>(item)));
        }
        [HttpPost]
        public async Task<IActionResult> AddItemToCart(int userId, [FromBody] CartItemCreateRequest request)
        {
            var item = _mapper.Map<CartItem>(request);
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
        [HttpPut("cart/{cartId}/item/{itemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartId, int itemId, [FromBody] int newQuantity)
        {
            try
            {
                await _cartItemService.UpdateCartItemQuantityAsync(cartId, itemId, newQuantity);
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

        [HttpDelete("cart/{cartId}/item/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int itemId)
        {
            try
            {
                await _cartItemService.RemoveItemFromCartAsync(cartId, itemId);
                return NoContent();
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
        
    }
}
