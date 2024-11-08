using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Cart;

namespace TP4SCS.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateCartAsync(int userId);
        Task ClearCartAsync(int cartId);
        Task<decimal> GetCartTotalAsync(int cartId);
        Task CheckoutAsync(HttpClient httpClient, CheckoutRequest request);
        Task<decimal> GetFeeShip(HttpClient httpClient, int addressId, int branchId, int quantity);
        //Task UpdateCartAsync(Cart cart, int existingCartId);
    }
}
