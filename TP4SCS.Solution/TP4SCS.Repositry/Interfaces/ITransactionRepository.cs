using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Library.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        // Lấy tất cả giao dịch
        Task<IEnumerable<Transaction>> GetTransactionsAsync(OrderByEnum orderBy = OrderByEnum.IdDesc);

        // Lấy giao dịch theo ID
        Task<Transaction?> GetTransactionByIdAsync(int id);

        // Lấy tất cả giao dịch theo AccountId
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);

        // Tạo mới giao dịch
        Task CreateTransactionAsync(Transaction transaction);

        // Cập nhật giao dịch
        Task UpdateTransactionAsync(Transaction transaction);

        // Xóa giao dịch theo ID
        Task DeleteTransactionAsync(int id);
    }
}