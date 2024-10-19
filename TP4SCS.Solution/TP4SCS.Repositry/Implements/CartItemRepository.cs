using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class CartItemRepository : GenericRepoistory<CartItem>, ICartItemRepository
    {
        private readonly ICartRepository _cartRepository;
        private readonly IServiceRepository _serviceRepository;

        public CartItemRepository(Tp4scsDevDatabaseContext dbContext, ICartRepository cartRepository
            , IServiceRepository serviceRepository) : base(dbContext)
        {
            _cartRepository = cartRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task AddItemToCartAsync(int userId, CartItem item)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(userId);
            }
            // Kiểm tra xem item đã tồn tại trong giỏ hàng chưa
            var existingItem = cart.CartItems.SingleOrDefault(i => i.ServiceId == item.ServiceId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                var service = await _serviceRepository.GetServiceByIdAsync(item.ServiceId);
                if (service == null)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} không tìm thấy.");
                }
                if (service.Status.ToUpper() == StatusConstants.Inactive)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} đã ngừng hoạt động.");
                }
                item.Price = service.Price;
                item.CartId = cart.Id;
                cart.CartItems.Add(item);
            }
            cart.TotalPrice = await _cartRepository.GetCartTotalAsync(cart.Id);
            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int itemId)
        {
            return await GetByIDAsync(itemId);
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsAsync(int cartId)
        {
            return await GetAsync(filter: item => item.CartId == cartId);
        }

        public async Task RemoveItemFromCartAsync(int cartId, int itemId)
        {
            var itemToRemove = await GetAsync(filter: item => item.CartId == cartId && item.Id == itemId);

            if (itemToRemove != null && itemToRemove.Any())
            {
                foreach (var item in itemToRemove)
                {
                    await DeleteAsync(item);
                    var cart = await _cartRepository.GetCartByIdAsync(cartId);

                    if (cart == null)
                    {
                        throw new KeyNotFoundException($"Cart with cartId {cartId} not found.");
                    }
                    cart.TotalPrice = await _cartRepository.GetCartTotalAsync(cartId);
                    await _cartRepository.UpdateCartAsync(cart);
                }
            }
            else
            {
                throw new KeyNotFoundException($"CartItem with itemId {itemId} not found in cartId {cartId}.");
            }
        }

        public async Task UpdateCartItemQuantityAsync(int cartId, int itemId, int newQuantity)
        {
            var cartItem = await GetAsync(filter: item => item.CartId == cartId && item.Id == itemId);

            if (cartItem != null && cartItem.Any())
            {
                var itemToUpdate = cartItem.FirstOrDefault();

                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = newQuantity;
                    await UpdateAsync(itemToUpdate);
                    var cart = await _cartRepository.GetCartByIdAsync(cartId);

                    if (cart == null)
                    {
                        throw new KeyNotFoundException($"Cart with cartId {cartId} not found.");
                    }
                    cart.TotalPrice = await _cartRepository.GetCartTotalAsync(cartId);
                    await _cartRepository.UpdateCartAsync(cart);

                }
            }
            else
            {
                throw new KeyNotFoundException($"CartItem with itemId {itemId} not found in cartId {cartId}.");
            }
        }
    }
}
