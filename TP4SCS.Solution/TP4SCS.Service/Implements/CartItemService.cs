using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IServiceService _serviceService;

        public CartItemService(ICartItemRepository cartItemRepository, IServiceService serviceService)
        {
            _cartItemRepository = cartItemRepository;
            _serviceService = serviceService;
        }
        public async Task AddItemToCartAsync(int userId, CartItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Mục trong giỏ hàng không được để trống.");
            }

            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.", nameof(item.Quantity));
            }

            await _cartItemRepository.AddItemToCartAsync(userId, item);
        }
        public async Task AddItemsToCartAsync(int userId, List<CartItem> items)
        {
            if (!items.Any())
            {
                throw new ArgumentNullException("Mục trong giỏ hàng không được để trống.");
            }

            if (items.Any(i => i.Quantity<=0))
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.");
            }

            await _cartItemRepository.AddItemsToCartAsync(userId, items);
        }
        public async Task<CartItem?> GetCartItemByIdAsync(int itemId)
        {
            return await _cartItemRepository.GetCartItemByIdAsync(itemId);
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsAsync(int cartId)
        {
            return await _cartItemRepository.GetCartItemsAsync(cartId);
        }

        public async Task RemoveItemsFromCartAsync(int[] itemIds)
        {
            await _cartItemRepository.RemoveItemsFromCartAsync(itemIds);
        }

        public async Task UpdateCartItemQuantityAsync(int itemId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.", nameof(newQuantity));
            }
            await _cartItemRepository.UpdateCartItemQuantityAsync(itemId, newQuantity);
        }

        public async Task<decimal> CalculateCartItemsTotal(List<int> cartItemIds)
        {
            decimal totalPrice = 0;
            var cartItems = new List<CartItem>();
            foreach (var id in cartItemIds)
            {
                CartItem? cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
                if (cartItem == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy mặt hàng giỏ hàng với ID {id}.");
                }
                cartItems.Add(cartItem);
            }

            foreach (var cartItem in cartItems)
            {
                decimal servicePrice = await _serviceService.GetServiceFinalPriceAsync(cartItem.ServiceId!.Value);

                if (servicePrice < 0 && cartItem.ServiceId.HasValue)
                {
                    throw new InvalidOperationException($"Giá dịch vụ không hợp lệ cho serviceId {cartItem.ServiceId.Value}.");
                }

                totalPrice += servicePrice * cartItem.Quantity;
            }

            return totalPrice;
        }

    }
}
