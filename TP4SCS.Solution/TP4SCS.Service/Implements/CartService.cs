using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class CartService : ICartService
    {
        private ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task ClearCartAsync(int cartId)
        {
            await _cartRepository.ClearCartAsync(cartId);
        }

        public async Task<Cart> CreateCartAsync(int userId)
        {
            return await _cartRepository.CreateCartAsync(userId);
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            return await _cartRepository.GetCartTotalAsync(cartId);
        }

        public async Task UpdateCartAsync(Cart cart, int existingCartId)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "Cart cannot be null.");
            }
            if (cart.TotalPrice < 0)
            {
                throw new ArgumentException("Total Price cannot below 0 VND.");
            }
            var existingCart = await _cartRepository.GetCartByIdAsync(existingCartId);
            if (existingCart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {existingCartId} not found.");
            }
            existingCart.TotalPrice = cart.TotalPrice;
            await _cartRepository.UpdateCartAsync(cart);
        }
    }
}
