using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Implements;

namespace TP4SCS.Library.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        // Lấy tất cả giao dịch
        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(
            OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            // Tạo truy vấn cơ bản
            var query = _dbContext.Transactions
                .AsQueryable();

            // Áp dụng sắp xếp theo OrderByEnum
            query = orderBy switch
            {
                OrderByEnum.IdAsc => query.OrderBy(t => t.Id),
                OrderByEnum.IdDesc => query.OrderByDescending(t => t.Id),
                _ => query.OrderByDescending(t => t.Id)
            };
            return await query.ToListAsync();
        }


        // Lấy giao dịch theo ID
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _dbContext.Transactions
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Lấy tất cả giao dịch theo AccountId
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _dbContext.Transactions
                .Where(t => t.AccountId == accountId)
                .ToListAsync();
        }

        // Tạo mới giao dịch
        public async Task CreateTransactionAsync(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            await InsertAsync(transaction);
        }

        // Cập nhật giao dịch
        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            await UpdateAsync(transaction);
        }

        // Xóa giao dịch theo ID
        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(t => t.Id == id);
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            await DeleteAsync(id);
        }
    }
}
