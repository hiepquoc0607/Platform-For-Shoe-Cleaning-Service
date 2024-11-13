using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Library.Services
{
    public interface ITransactionService
    {
        // Lấy tất cả giao dịch
        Task<PagedResponse<Transaction>> GetTransactionsAsync(int pageIndex = 1, int pageSize = 10, OrderByEnum orderBy = OrderByEnum.IdDesc);

        // Lấy giao dịch theo ID
        Task<Transaction?> GetTransactionByIdAsync(int id);

        // Lấy tất cả giao dịch theo AccountId
        Task<PagedResponse<Transaction>> GetTransactionsByAccountIdAsync(int accountId, int pageIndex = 1, int pageSize = 10);

        // Tạo mới giao dịch
        Task CreateTransactionAsync(Transaction transaction);

        // Cập nhật giao dịch
        Task UpdateTransactionAsync(int existingTransactionId, Transaction transaction);

        // Xóa giao dịch theo ID
        Task DeleteTransactionAsync(int id);
    }
}