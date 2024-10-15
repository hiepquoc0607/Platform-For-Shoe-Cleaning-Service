using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class CartRepository : GenericRepoistory<Cart>, ICartRepository
    {
        public CartRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null) return;

            // Xóa từng CartItem
            _dbContext.CartItems.RemoveRange(cart.CartItems);
            cart.TotalPrice = 0;
            await _dbContext.SaveChangesAsync(); // Lưu thay đổi
        }


        public async Task<Cart> CreateCartAsync(int userId)
        {
            var cart = new Cart
            {
                AccountId = userId,
                TotalPrice = 0
            };
            await InsertAsync(cart);
            return cart;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int accountId)
        {
            var result = await GetAsync(filter: c => c.AccountId == accountId,
                includeProperties: "CartItems");
            return result?.FirstOrDefault();
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _dbContext.Carts.Include(c => c.CartItems).SingleOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            Cart? cart = await GetCartByIdAsync(cartId);


            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return 0;
            }

            decimal totalPrice = cart.CartItems
                .Sum(item => item.Price * item.Quantity);

            return totalPrice;
        }


        public async Task UpdateCartAsync(Cart cart)
        {
            await UpdateAsync(cart);
        }

    }
}
