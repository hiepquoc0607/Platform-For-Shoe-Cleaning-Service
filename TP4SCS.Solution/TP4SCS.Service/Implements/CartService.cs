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
                throw new ArgumentNullException(nameof(cart), "Giỏ hàng không được để trống.");
            }
            if (cart.TotalPrice < 0)
            {
                throw new ArgumentException("Tổng giá không được nhỏ hơn 0 VND.");
            }
            var existingCart = await _cartRepository.GetCartByIdAsync(existingCartId);
            if (existingCart == null)
            {
                throw new KeyNotFoundException($"Giỏ hàng với ID {existingCartId} không tìm thấy.");
            }
            existingCart.TotalPrice = cart.TotalPrice;
            await _cartRepository.UpdateCartAsync(cart);
        }
    }
}
