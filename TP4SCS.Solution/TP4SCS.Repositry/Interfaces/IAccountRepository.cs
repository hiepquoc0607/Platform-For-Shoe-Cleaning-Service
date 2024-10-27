using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;

namespace TP4SCS.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<IEnumerable<Account>?> GetAccountsAsync(GetAccountRequest getAccountRequest);

        Task<Account?> GetAccountByIdAsync(int id);

        Task<Account?> GetAccountByIdForAdminAsync(int id);

        Task<Account?> GetAccountLoginByEmailAsync(string email);

        Task<Account?> GetAccountByEmailAsync(string email);

        Task<bool> IsEmailExistedAsync(string email);

        Task<bool> IsPhoneExistedAsync(string phone);

        Task<int> GetAccountMaxIdAsync();

        Task<int> CountAccountDataAsync();

        Task CreateAccountAsync(Account account);

        Task UpdateAccountAsync(Account account);
    }
}
