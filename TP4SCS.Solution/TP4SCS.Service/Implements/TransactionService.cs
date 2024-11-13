using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Repositories;

namespace TP4SCS.Library.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // Lấy tất cả giao dịch
        public async Task<PagedResponse<Transaction>> GetTransactionsAsync(int pageIndex = 1, int pageSize = 10, OrderByEnum orderBy = OrderByEnum.IdDesc)
        {
            // Lấy tất cả giao dịch từ repository
            var transactions = await _transactionRepository.GetTransactionsAsync(orderBy);

            // Áp dụng phân trang: Skip và Take
            var totalCount = transactions.Count();
            var pagedTransactions = transactions
                .Skip((pageIndex - 1) * pageSize)  // Bỏ qua các phần tử trước trang hiện tại
                .Take(pageSize);                   // Lấy đúng số lượng giao dịch cho trang hiện tại

            // Trả về PagedResponse với danh sách giao dịch và tổng số lượng
            return new PagedResponse<Transaction>(pagedTransactions, totalCount, pageIndex, pageSize);
        }


        // Lấy giao dịch theo ID
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }

        public async Task<PagedResponse<Transaction>> GetTransactionsByAccountIdAsync(int accountId, int pageIndex = 1, int pageSize = 10)
        {
            // Lấy tất cả giao dịch từ repository theo AccountId
            var transactions = await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);

            // Áp dụng phân trang: Skip và Take
            var totalCount = transactions.Count();
            var pagedTransactions = transactions
                .Skip((pageIndex - 1) * pageSize)  // Bỏ qua các phần tử trước trang hiện tại
                .Take(pageSize);                   // Lấy đúng số lượng giao dịch cho trang hiện tại

            // Trả về PagedResponse với danh sách giao dịch và tổng số lượng
            return new PagedResponse<Transaction>(pagedTransactions, totalCount, pageIndex, pageSize);
        }


        // Tạo mới giao dịch
        public async Task CreateTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null");
            }

            await _transactionRepository.CreateTransactionAsync(transaction);
        }

        // Cập nhật giao dịch
        public async Task UpdateTransactionAsync(int existingTransactionId, Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null");
            }
            var existingTransaction = await _transactionRepository.GetTransactionByIdAsync(existingTransactionId);
            if (existingTransaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), $"Transaction with ID {existingTransactionId} not found.");
            }
            await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        // Xóa giao dịch theo ID
        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
        }
    }
}