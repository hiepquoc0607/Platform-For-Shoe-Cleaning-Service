using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        public async Task AddItemToCartAsync(int userId, CartItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cart item cannot be null.");
            }

            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(item.Quantity));
            }

            if (item.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.", nameof(item.Price));
            }

            await _cartItemRepository.AddItemToCartAsync(userId, item);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int itemId)
        {
            return await _cartItemRepository.GetCartItemByIdAsync(itemId);
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsAsync(int cartId)
        {
            return await _cartItemRepository.GetCartItemsAsync(cartId);
        }

        public async Task RemoveItemFromCartAsync(int cartId, int itemId)
        {
            await _cartItemRepository.RemoveItemFromCartAsync(cartId, itemId);
        }

        public async Task UpdateCartItemQuantityAsync(int cartId, int itemId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
            }
            await _cartItemRepository.UpdateCartItemQuantityAsync(cartId, itemId, newQuantity);
        }
    }
}
