using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
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
            var existingItem = cart.CartItems.SingleOrDefault(i => i.ServiceId == item.ServiceId && i.BranchId ==item.BranchId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                var service = await _serviceRepository.GetServiceByIdAsync(item.ServiceId!.Value);
                if (service == null)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} không tìm thấy.");
                }
                if (service.Status.ToUpper() == StatusConstants.Inactive)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} đã ngừng hoạt động.");
                }
                item.CartId = cart.Id;
                cart.CartItems.Add(item);
            }
            await _cartRepository.UpdateCartAsync(cart);
        }
        public async Task<IEnumerable<CartItem>> GetCartItemsByIdsAsync(int[] cartItemIds)
        {
            return await _dbContext.CartItems
                .Where(item => cartItemIds.Contains(item.Id))
                .Include(item => item.Service)
                .ToListAsync();
        }
        public async Task<CartItem?> GetCartItemByIdAsync(int itemId)
        {
            return await GetByIDAsync(itemId);
        }

        public async Task<IEnumerable<CartItem>?> GetCartItemsAsync(int cartId)
        {
            return await GetAsync(filter: item => item.CartId == cartId);
        }

        public async Task RemoveItemsFromCartAsync(int[] itemIds)
        {
            // Tìm tất cả các mục có Id nằm trong mảng itemIds
            var itemsToRemove = await _dbContext.CartItems
                .Where(item => itemIds.Contains(item.Id))
                .ToListAsync();

            if (itemsToRemove.Any())
            {
                // Xóa các mục đã tìm thấy
                _dbContext.RemoveRange(itemsToRemove);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("No CartItems found with the specified itemIds.");
            }
        }

        public async Task UpdateCartItemQuantityAsync(int itemId, int newQuantity)
        {
            var itemToUpdate = await _dbContext.CartItems
                .SingleOrDefaultAsync(item => item.Id == itemId);

            if (itemToUpdate != null )
            {
                itemToUpdate.Quantity = newQuantity;
                await UpdateAsync(itemToUpdate);
            }
            else
            {
                throw new KeyNotFoundException($"CartItem with itemId {itemId} not found.");
            }
        }
    }
}
