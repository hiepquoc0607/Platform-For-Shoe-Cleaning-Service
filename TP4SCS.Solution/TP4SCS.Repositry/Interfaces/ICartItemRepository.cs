using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface ICartItemRepository
    {
        Task AddItemToCartAsync(int cartId, CartItem item);
        Task UpdateCartItemQuantityAsync(int itemId, int newQuantity);
        Task RemoveItemFromCartAsync(int cartId, int itemId);
        Task<IEnumerable<CartItem>?> GetCartItemsAsync(int cartId);
        Task<CartItem?> GetCartItemByIdAsync(int itemId);
    }
}
