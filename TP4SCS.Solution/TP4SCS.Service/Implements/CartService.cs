using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IServiceService _serviceService;

        public CartService(ICartRepository cartRepository, IServiceService serviceService)
        {
            _cartRepository = cartRepository;
            _serviceService = serviceService;
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
            Cart? cart = await _cartRepository.GetCartByIdAsync(cartId);
            decimal totalPrice = 0;

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return totalPrice;
            }

            foreach (var item in cart.CartItems)
            {
                decimal servicePrice = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId);

                totalPrice += servicePrice * item.Quantity;
            }

            return totalPrice;
        }


        //public async Task UpdateCartAsync(Cart cart, int existingCartId)
        //{
        //    if (cart == null)
        //    {
        //        throw new ArgumentNullException(nameof(cart), "Giỏ hàng không được để trống.");
        //    }
        //    var existingCart = await _cartRepository.GetCartByIdAsync(existingCartId);
        //    if (existingCart == null)
        //    {
        //        throw new KeyNotFoundException($"Giỏ hàng với ID {existingCartId} không tìm thấy.");
        //    }
        //    await _cartRepository.UpdateCartAsync(cart);
        //}
    }
}
