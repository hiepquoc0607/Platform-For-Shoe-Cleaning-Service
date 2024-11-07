using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly Util _util;

        public AccountService(IAccountRepository accountRepository, IAuthService authService, IMapper mapper, Util util)
        {
            _accountRepository = accountRepository;
            _authService = authService;
            _mapper = mapper;
            _util = util;
        }

        //Create Account
        public async Task<ApiResponse<AccountResponse>> CreateAccountAsync(CustomerRegisterRequest createAccountRequest)
        {
            var passwordError = _util.CheckPasswordErrorType(createAccountRequest.Password);

            var passwordErrorMessages = new Dictionary<string, string>
            {
                { "Number", "Mật khẩu phải chứa ít nhất 1 kí tự số!" },
                { "Lower", "Mật khẩu phải chứa ít nhất 1 kí tự viết thường!" },
                { "Upper", "Mật khẩu phải chứa ít nhất 1 kí tự viết hoa!" },
                { "Special", "Mật khẩu phải chứa ít nhất 1 kí tự đặc biệt (!, @, #, $,...)!" },
            };

            if (passwordErrorMessages.TryGetValue(passwordError, out var message))
            {
                return new ApiResponse<AccountResponse>("error", 400, message);
            }

            var isEmailExisted = await _accountRepository.IsEmailExistedAsync(createAccountRequest.Email.Trim().ToLower());

            if (isEmailExisted == true)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Email đã được sử dụng!");
            }

            var isPhoneExisted = await _accountRepository.IsPhoneExistedAsync(createAccountRequest.Phone.Trim());

            if (isPhoneExisted == true)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Số điện thoại đã được sử dụng!");
            }

            //if (!_util.CheckAccountRole(createAccountRequest.Role.Trim()))
            //{
            //    return new ApiResponse<AccountResponse>("error", 400, "Role Không Phù Hợp!");
            //}

            //createAccountRequest.Role = createAccountRequest.Role.ToUpper();

            var newAccount = _mapper.Map<Account>(createAccountRequest);

            newAccount.PasswordHash = _util.HashPassword(createAccountRequest.Password);

            try
            {
                await _accountRepository.InsertAsync(newAccount);

                var maxId = await GetAccountMaxIdAsync();

                var newAcc = await GetAccountByIdAsync(maxId);

                return new ApiResponse<AccountResponse>("success", "Tạo Tài Khoản Thành Công!", newAcc.Data);
            }
            catch (Exception)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Tạo Tài Khoản Thất Bại!");
            }
        }


        //Delete Account
        public async Task<ApiResponse<AccountResponse>> DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Tài Khoản Không Tồn Tại!");
            }

            account.Status = "INACTIVE";

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new ApiResponse<AccountResponse>("success", "Xoá Tài Khoản Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Xoá Tài Khoản Thất Bại!");
            }

        }

        public async Task<ApiResponse<AccountResponse?>> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.GetAccountLoginByEmailAsync(email);

            if (account == null)
            {
                return new ApiResponse<AccountResponse?>("error", 400, "Tài Khoản Không Tồn Tại!");
            }

            var data = _mapper.Map<AccountResponse>(account);

            return new ApiResponse<AccountResponse?>("success", "Lấy dữ liệu thành công!", data);
        }

        //Get Account By Id
        public async Task<ApiResponse<AccountResponse?>> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new ApiResponse<AccountResponse?>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            account.Role = _util.TranslateAccountRole(account.Role);
            account.Status = _util.TranslateAccountStatus(account.Status);

            var data = _mapper.Map<AccountResponse>(account);

            return new ApiResponse<AccountResponse?>("success", "Lấy dữ liệu thành công!", data);
        }

        //Get Account Max Id
        public async Task<int> GetAccountMaxIdAsync()
        {
            return await _accountRepository.GetAccountMaxIdAsync();
        }

        //Get Accounts
        public async Task<ApiResponse<IEnumerable<AccountResponse>?>> GetAccountsAsync(GetAccountRequest getAccountRequest)
        {
            var (accounts, pagination) = await _accountRepository.GetAccountsAsync(getAccountRequest);

            if (accounts == null)
            {
                return new ApiResponse<IEnumerable<AccountResponse>?>("error", 404, "Tài Khoản Trống!");
            }

            //Paging caculate
            int totalData = await _accountRepository.CountAccountDataAsync();

            var data = accounts.Adapt<IEnumerable<AccountResponse>>();

            return new ApiResponse<IEnumerable<AccountResponse>?>("success", "Lấy dữ liệu thành công!", data, 200, pagination);
        }

        //Update Account
        public async Task<ApiResponse<AccountResponse>> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest)
        {
            var oldAccount = await _accountRepository.GetAccountByIdAsync(id);

            if (oldAccount == null)
            {
                return new ApiResponse<AccountResponse>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            var newAccount = _mapper.Map(updateAccountRequest, oldAccount);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new ApiResponse<AccountResponse>("success", "Cập Nhập Tài Khoản Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Cập Nhập Tài Khoản Thất Bại!");
            }
        }

        //Update Account Password
        public async Task<ApiResponse<AccountResponse>> UpdateAccountPasswordAsync(int id, UpdateAccountPasswordRequest updateAccountPasswordRequest)
        {
            var oldAccount = await _accountRepository.GetAccountByIdAsync(id);

            if (oldAccount == null)
            {
                return new ApiResponse<AccountResponse>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            var oldPass = _util.HashPassword(updateAccountPasswordRequest.OldPassword);

            if (!_util.CompareHashedPassword(oldPass, oldAccount.PasswordHash))
            {
                return new ApiResponse<AccountResponse>("error", 400, "Mật Khẩu Không Đúng!");
            }

            var newPass = _util.HashPassword(updateAccountPasswordRequest.NewPassword);
            oldAccount.PasswordHash = newPass;

            try
            {
                await _accountRepository.UpdateAccountAsync(oldAccount);

                return new ApiResponse<AccountResponse>("success", "Đổi Mật Khẩu Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Đổi Mật Khẩu Thất Bại!");
            }
        }

        //Update Account Status For Admin
        public async Task<ApiResponse<AccountResponse>> UpdateAccountStatusForAdminAsync(int id, UpdateStatusRequest updateStatusRequest)
        {
            var account = await _accountRepository.GetAccountByIdForAdminAsync(id);

            if (account == null)
            {
                return new ApiResponse<AccountResponse>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            if (!_util.CheckStatusForAdmin(account.Status, updateStatusRequest.Status))
            {
                return new ApiResponse<AccountResponse>("error", 400, "Trạng Thái Tài Khoản Trùng Lập!");
            }

            account.Status = updateStatusRequest.Status.ToUpper().Trim() switch
            {
                "INACTIVE" => "INACTIVE",
                "SUSPENDED" => "SUSPENDED",
                _ => "ACTIVE"
            };

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new ApiResponse<AccountResponse>("success", "Cập Nhập Trạng Thái Tài Khoản Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AccountResponse>("error", 400, "Cập Nhập Trạng Thái Tài Khoản Thất Bại!");
            }
        }
    }

}