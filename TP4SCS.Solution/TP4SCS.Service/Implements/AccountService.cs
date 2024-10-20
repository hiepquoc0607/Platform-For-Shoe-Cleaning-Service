using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils;
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

        public async Task<Result> CreateAccountAsync(CreateAccountRequest createAccountRequest)
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
                return new Result { IsSuccess = false, Message = message };
            }

            var isEmailExisted = await _accountRepository.IsEmailExistedAsync(createAccountRequest.Email.ToLower());

            if (isEmailExisted == true)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Email đã được sử dụng!" };
            }

            var isPhoneExisted = await _accountRepository.IsPhoneExistedAsync(createAccountRequest.Phone);

            if (isPhoneExisted == true)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Số điện thoại đã được sử dụng!" };
            }

            if (!_util.CheckAccountRole(createAccountRequest.Role))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Invalid Account Role!" };
            }

            createAccountRequest.Role = createAccountRequest.Role.ToUpper();

            var newAccount = _mapper.Map<Account>(createAccountRequest);

            newAccount.PasswordHash = _util.HashPassword(createAccountRequest.Password);

            try
            {
                await _accountRepository.InsertAsync(newAccount);

                return new Result { IsSuccess = true, Message = "Tạo Tài Khoản Thành Công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Tạo Tài Khoản Thất Bại!" };
            }
        }

        public async Task<Result> DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 404, Message = "Tài Khoản Không Tồn Tại!" };
            }

            account.Status = "INACTIVE";

            //var newAccount = _mapper.Map<Account>(account);

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new Result { IsSuccess = true, Message = "Xoá Tài Khoản Thành Công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Xoá Tài Khoản Thất Bại!" };
            }

        }

        public async Task<AccountResponse?> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return null;
            }

            account.Role = _util.TranslateAccountRole(account.Role);
            account.Status = _util.TranslateAccountStatus(account.Status);

            var result = _mapper.Map<AccountResponse>(account);

            return result;
        }

        public async Task<int> GetAccountMaxIdAsync()
        {
            return await _accountRepository.GetAccountMaxIdAsync();
        }

        public async Task<IEnumerable<AccountResponse>?> GetAccountsAsync(GetAccountRequest getAccountRequest)
        {
            var accounts = await _accountRepository.GetAccountsAsync();

            if (accounts == null)
            {
                return null;
            }

            //Search
            if (!string.IsNullOrEmpty(getAccountRequest.SearchKey))
            {
                accounts = accounts.Where(r => r.FullName.ToLower().Contains(getAccountRequest.SearchKey.ToLower()) || r.Email.ToLower().Contains(getAccountRequest.SearchKey.ToLower()));
            }

            //Sort
            if (!string.IsNullOrEmpty(getAccountRequest.SortBy))
            {
                accounts = getAccountRequest.SortBy.ToUpper() switch
                {
                    "EMAIL" => getAccountRequest.IsDecsending
                                ? accounts.OrderByDescending(a => a.Email)
                                : accounts.OrderBy(a => a.Email),
                    "FULLNAME" => getAccountRequest.IsDecsending
                                  ? accounts.OrderByDescending(a => a.FullName)
                                  : accounts.OrderBy(a => a.FullName),
                    "STATUS" => getAccountRequest.IsDecsending
                                  ? accounts.OrderByDescending(a => a.Status)
                                  : accounts.OrderBy(a => a.Status),
                    _ => accounts
                };
            }

            //Paging
            int skipNum = (getAccountRequest.PageNum - 1) * getAccountRequest.PageSize;
            accounts.Skip(skipNum).Take(getAccountRequest.PageSize).ToList();

            var result = accounts.Adapt<IEnumerable<AccountResponse>>();

            return result;
        }

        public async Task<Result> LoginAsync(LoginRequest loginRequest)
        {
            var account = await _accountRepository.GetAccountLoginByEmailAsync(loginRequest.Email);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Email Không Tồn Tại!" };
            }

            if (!_util.CompareHashedPassword(loginRequest.Password, account.PasswordHash))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Mật Khẩu Không Đúng!" };
            }

            var token = _authService.GenerateToken(account);

            return new Result { IsSuccess = true, Token = token };
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(resetPasswordRequest.Email);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Email Không Tồn Tại!" };
            }

            if (!resetPasswordRequest.NewPassword.Equals(resetPasswordRequest.ConfirmPassword))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Mật Khẩu Xác Nhận Không Trùng!" };
            }

            account.PasswordHash = _util.HashPassword(resetPasswordRequest.NewPassword);

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new Result { IsSuccess = true, Message = "Đặt Lại Mật Khẩu Thành Công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Đặt Lại Mật Khẩu Thất Bại!" };
            }
        }

        public async Task<Result> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest)
        {
            var oldAccount = await _accountRepository.GetAccountByIdAsync(id);

            if (oldAccount == null)
            {
                return new Result { IsSuccess = false, StatusCode = 404, Message = "Tài Khoản Không Tồn Tại!" };
            }

            var newAccount = _mapper.Map(updateAccountRequest, oldAccount);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new Result { IsSuccess = true, Message = "Cập Nhập Tài Khoản Thành Công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Cập Nhập Tài Khoản Thất Bại!" };
            }

        }

        public async Task<Result> UpdateAccountStatusForAdminAsync(int id, string status)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 404, Message = "Tài Khoản Không Tồn Tại!!" };
            }

            if (!_util.CheckAccountStatusForAdmin(account.Status, status))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Trạng Thái Tài Khoản Trùng Lập!" };
            }

            account.Status = status.ToUpper() switch
            {
                "INACTIVE" => "INACTIVE",
                "SUSPENDED" => "SUSPENDED",
                _ => "ACTIVE"
            };

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new Result { IsSuccess = true, Message = "Cập Nhập Trạng Thái Tài Khoản Thành Công!" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Cập Nhập Trạng Thái Tài Khoản Thất Bại!" };
            }
        }
    }

}