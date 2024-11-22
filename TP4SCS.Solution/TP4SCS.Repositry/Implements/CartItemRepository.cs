using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {


        public CartItemRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {

        }

        public async Task AddItemToCartAsync(int userId, CartItem item)
        {
            // Lấy giỏ hàng của người dùng từ DbContext
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems) // Đảm bảo bao gồm các CartItems
                .FirstOrDefaultAsync(c => c.AccountId == userId);

            // Nếu giỏ hàng không tồn tại, tạo một giỏ hàng mới
            if (cart == null)
            {
                cart = new Cart { AccountId = userId };
                await _dbContext.Carts.AddAsync(cart);
            }

            // Kiểm tra xem item đã tồn tại trong giỏ hàng chưa
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ServiceId == item.ServiceId && i.BranchId == item.BranchId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity; // Cập nhật số lượng
            }
            else
            {
                var service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == item.ServiceId!.Value);
                if (service == null)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} không tìm thấy.");
                }
                if (service.Status.ToUpper() == StatusConstants.UNAVAILABLE)
                {
                    throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} đã ngừng hoạt động.");
                }

                // Thêm item vào giỏ hàng
                item.CartId = cart.Id;
                cart.CartItems.Add(item);
            }

            // Cập nhật giỏ hàng trong DbContext
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddItemsToCartAsync(int userId, List<CartItem> items)
        {
            // Lấy giỏ hàng của người dùng
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.AccountId == userId);

            // Nếu giỏ hàng không tồn tại, tạo mới
            if (cart == null)
            {
                cart = new Cart { AccountId = userId };
                await _dbContext.Carts.AddAsync(cart);
                await _dbContext.SaveChangesAsync(); // Lưu lại để có CartId
            }

            foreach (var item in items)
            {
                // Kiểm tra hợp lệ của CartItem
                if (item.MaterialId.HasValue && !item.ServiceId.HasValue)
                {
                    throw new InvalidOperationException("Material phải được liên kết với một Service.");
                }

                // Kiểm tra nếu item đã tồn tại
                var existingItem = cart.CartItems.FirstOrDefault(i =>
                    i.ServiceId == item.ServiceId &&
                    i.BranchId == item.BranchId &&
                    i.MaterialId == item.MaterialId);

                if (existingItem != null)
                {
                    // Nếu tồn tại, cập nhật số lượng
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    // Kiểm tra Service nếu chỉ có ServiceId
                    if (item.ServiceId.HasValue && !item.MaterialId.HasValue)
                    {
                        var service = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == item.ServiceId.Value);
                        if (service == null)
                        {
                            throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} không tồn tại.");
                        }
                        if (service.Status.ToUpper() == StatusConstants.UNAVAILABLE)
                        {
                            throw new InvalidOperationException($"Dịch vụ với ID {item.ServiceId} đã ngừng hoạt động.");
                        }
                    }

                    // Kiểm tra Material nếu có cả MaterialId và ServiceId
                    if (item.ServiceId.HasValue && item.MaterialId.HasValue)
                    {
                        var material = await _dbContext.Materials.SingleOrDefaultAsync(m => m.Id == item.MaterialId.Value);
                        if (material == null)
                        {
                            throw new InvalidOperationException($"Nguyên liệu với ID {item.MaterialId} không tồn tại.");
                        }
                        if (material.Status.ToUpper() == StatusConstants.UNAVAILABLE)
                        {
                            throw new InvalidOperationException($"Nguyên liệu với ID {item.MaterialId} đã ngừng hoạt động.");
                        }

                        // Đảm bảo Material liên kết đúng với ServiceId
                        var isLinked = await _dbContext.ServiceMaterials
                            .AnyAsync(sm => sm.MaterialId == item.MaterialId && sm.ServiceId == item.ServiceId);
                        if (!isLinked)
                        {
                            throw new InvalidOperationException($"Nguyên liệu với ID {item.MaterialId} không liên kết với Dịch vụ {item.ServiceId}.");
                        }
                    }

                    // Thêm item mới vào giỏ
                    item.CartId = cart.Id;
                    cart.CartItems.Add(item);
                }
            }

            // Lưu thay đổi
            await _dbContext.SaveChangesAsync();
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

            if (itemToUpdate != null)
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
