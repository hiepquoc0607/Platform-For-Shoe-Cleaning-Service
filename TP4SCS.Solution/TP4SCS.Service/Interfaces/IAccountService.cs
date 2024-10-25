using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Result<IEnumerable<AccountResponse>?>> GetAccountsAsync(GetAccountRequest getAccountRequest);

        Task<Result<AccountResponse?>> GetAccountByIdAsync(int id);

        Task<int> GetAccountMaxIdAsync();

        Task<Result<AccountResponse>> CreateAccountAsync(CreateAccountRequest createAccountRequest);

        Task<Result<AccountResponse>> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest);

        Task<Result<AccountResponse>> UpdateAccountStatusForAdminAsync(int id, string status);

        Task<Result<AccountResponse>> DeleteAccountAsync(int id);

    }
}
