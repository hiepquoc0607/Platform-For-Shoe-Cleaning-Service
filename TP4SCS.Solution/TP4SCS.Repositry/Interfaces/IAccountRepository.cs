using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Response.Account;

namespace TP4SCS.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<IEnumerable<AccountResponse>?> GetAccountsAsync();
        Task<AccountResponse?> GetAccountByIdAsync(int id);
        Task<Account?> GetAccountLoginByEmailAsync(string email);
        Task<AccountResponse?> GetAccountByEmailAsync(string email);
        Task<bool> IsEmailExistedAsync(string email);
        Task<bool> IsPhoneExistedAsync(string phone);
        Task<int> GetAccountMaxIdAsync();
        Task CreateAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
    }
}
