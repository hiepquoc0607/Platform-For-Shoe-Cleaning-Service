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
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    Account = _dbContext.Accounts
                    .AsNoTracking()
                    .Where(a => a.Id == t.AccountId)
                    .Select(a => new Account
                    {
                        Id = a.Id,
                        Email = a.Email,
                        FullName = a.FullName,
                        Phone = a.Phone,
                        Gender = a.Gender,
                        Dob = a.Dob,
                        ImageUrl = a.ImageUrl,
                        Status = a.Status,
                    })
                    .SingleOrDefault()!,
                    PackName = t.PackName,
                    Balance = t.Balance,
                    ProcessTime = t.ProcessTime,
                    Description = t.Description,
                    PaymentMethod = t.PaymentMethod,
                    Status = t.Status
                })
                .AsQueryable();

            // Áp dụng sắp xếp theo OrderByEnum
            query = orderBy switch
            {
                OrderByEnum.IdAsc => query.OrderBy(t => t.Id),
                OrderByEnum.IdDesc => query.OrderByDescending(t => t.Id),
                _ => query.OrderByDescending(t => t.Id)
            };

            return await query
                .OrderByDescending(t => t.ProcessTime)
                .ToListAsync();
        }


        // Lấy giao dịch theo ID
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _dbContext.Transactions
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        // Lấy tất cả giao dịch theo AccountId
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _dbContext.Transactions
                .Where(t => t.AccountId == accountId)
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    Account = _dbContext.Accounts
                    .AsNoTracking()
                    .Where(a => a.Id == t.AccountId)
                    .Select(a => new Account
                    {
                        Id = a.Id,
                        Email = a.Email,
                        FullName = a.FullName,
                        Phone = a.Phone,
                        Gender = a.Gender,
                        Dob = a.Dob,
                        ImageUrl = a.ImageUrl,
                        Status = a.Status,
                    })
                    .SingleOrDefault()!,
                    PackName = t.PackName,
                    Balance = t.Balance,
                    ProcessTime = t.ProcessTime,
                    Description = t.Description,
                    PaymentMethod = t.PaymentMethod,
                    Status = t.Status
                })
                .OrderByDescending(t => t.ProcessTime)
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

        public async Task<Transaction?> GetTransactionByIdForViewAsync(int id)
        {
            return await _dbContext.Transactions
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    Account = _dbContext.Accounts
                    .AsNoTracking()
                    .Where(a => a.Id == t.AccountId)
                    .Select(a => new Account
                    {
                        Id = a.Id,
                        Email = a.Email,
                        FullName = a.FullName,
                        Phone = a.Phone,
                        Gender = a.Gender,
                        Dob = a.Dob,
                        ImageUrl = a.ImageUrl,
                        Status = a.Status,
                    })
                    .SingleOrDefault()!,
                    PackName = t.PackName,
                    Balance = t.Balance,
                    ProcessTime = t.ProcessTime,
                    Description = t.Description,
                    PaymentMethod = t.PaymentMethod,
                    Status = t.Status
                })
                .SingleOrDefaultAsync(t => t.Id == id);
        }
    }
}
