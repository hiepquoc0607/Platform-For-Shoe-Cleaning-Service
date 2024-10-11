using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>?> GetAccountsAsync(GetAccountRequest getAccountRequest);
        Task<AccountResponse?> GetAccountByIdAsync(int id);
        Task<int> GetAccountMaxIdAsync();
        Task<Result> CreateAccountAsync(CreateAccountRequest createAccountRequest, RoleRequest roleRequest);
        Task<Result> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest);
        Task<Result> DeleteAccountAsync(int id);
        Task<Result> SuspendAccountAsync(int id);
        Task<Result> UnsuspendAccountAsync(int id);
    }
}
