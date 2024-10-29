using TP4SCS.Library.Models.Data;

namespace TP4SCS.Services.Interfaces
{
    public interface ICartItemService
    {
        Task AddItemToCartAsync(int userId, CartItem item);
        Task<CartItem?> GetCartItemByIdAsync(int itemId);
        Task<IEnumerable<CartItem>?> GetCartItemsAsync(int cartId);
        Task RemoveItemFromCartAsync(int cartId, int itemId);
        Task UpdateCartItemQuantityAsync(int itemId, int newQuantity);
        Task<decimal> CalculateCartItemsTotal(List<int> cartItemIds);
    }
}
