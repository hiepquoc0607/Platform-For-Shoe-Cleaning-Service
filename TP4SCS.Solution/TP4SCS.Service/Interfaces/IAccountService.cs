using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<IEnumerable<AccountResponse>?>> GetAccountsAsync(GetAccountRequest getAccountRequest);

        Task<ApiResponse<AccountResponse?>> GetAccountByIdAsync(int id);

        Task<int> GetAccountMaxIdAsync();

        Task<ApiResponse<AccountResponse>> CreateAccountAsync(CreateAccountRequest createAccountRequest);

        Task<ApiResponse<AccountResponse>> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest);

        Task<ApiResponse<AccountResponse>> UpdateAccountPasswordAsync(int id, UpdateAccountPasswordRequest updateAccountPasswordRequest);

        Task<ApiResponse<AccountResponse>> UpdateAccountStatusForAdminAsync(int id, UpdateStatusRequest updateStatusRequest);

        Task<ApiResponse<AccountResponse>> DeleteAccountAsync(int id);

    }
}
