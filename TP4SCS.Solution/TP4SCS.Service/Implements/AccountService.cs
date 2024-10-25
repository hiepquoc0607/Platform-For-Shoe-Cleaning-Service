using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
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

        //Create Account
        public async Task<Result<AccountResponse>> CreateAccountAsync(CreateAccountRequest createAccountRequest)
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
                return new Result<AccountResponse>("error", 400, message);
            }

            var isEmailExisted = await _accountRepository.IsEmailExistedAsync(createAccountRequest.Email.ToLower());

            if (isEmailExisted == true)
            {
                return new Result<AccountResponse>("error", 400, "Email đã được sử dụng!");
            }

            var isPhoneExisted = await _accountRepository.IsPhoneExistedAsync(createAccountRequest.Phone);

            if (isPhoneExisted == true)
            {
                return new Result<AccountResponse>("error", 400, "Số điện thoại đã được sử dụng!");
            }

            if (!_util.CheckAccountRole(createAccountRequest.Role))
            {
                return new Result<AccountResponse>("error", 400, "Role Không Phù Hợp!");
            }

            createAccountRequest.Role = createAccountRequest.Role.ToUpper();

            var newAccount = _mapper.Map<Account>(createAccountRequest);

            newAccount.PasswordHash = _util.HashPassword(createAccountRequest.Password);

            try
            {
                await _accountRepository.InsertAsync(newAccount);

                var maxId = await GetAccountMaxIdAsync();

                var newAcc = await GetAccountByIdAsync(maxId);

                return new Result<AccountResponse>("success", "Tạo Tài Khoản Thành Công!", newAcc.Data);
            }
            catch (Exception)
            {
                return new Result<AccountResponse>("error", 400, "Tạo Tài Khoản Thất Bại!");
            }
        }


        //Delete Account
        public async Task<Result<AccountResponse>> DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result<AccountResponse>("error", 400, "Tài Khoản Không Tồn Tại!");
            }

            account.Status = "INACTIVE";

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new Result<AccountResponse>("success", "Xoá Tài Khoản Thành Công!", null);
            }
            catch (Exception)
            {
                return new Result<AccountResponse>("error", 400, "Xoá Tài Khoản Thất Bại!");
            }

        }


        //Get Account By Id
        public async Task<Result<AccountResponse?>> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result<AccountResponse?>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            account.Role = _util.TranslateAccountRole(account.Role);
            account.Status = _util.TranslateAccountStatus(account.Status);

            var data = _mapper.Map<AccountResponse>(account);

            return new Result<AccountResponse?>("success", "Lấy dữ liệu thành công!", data);
        }

        //Get Account Max Id
        public async Task<int> GetAccountMaxIdAsync()
        {
            return await _accountRepository.GetAccountMaxIdAsync();
        }

        //Get Accounts
        public async Task<Result<IEnumerable<AccountResponse>?>> GetAccountsAsync(GetAccountRequest getAccountRequest)
        {
            var accounts = await _accountRepository.GetAccountsAsync();

            if (accounts == null)
            {
                return new Result<IEnumerable<AccountResponse>?>("error", 404, "Tài Khoản Trống!");
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

            var data = accounts.Adapt<IEnumerable<AccountResponse>>();

            return new Result<IEnumerable<AccountResponse>?>("success", "Lấy dữ liệu thành công!", data);
        }

        //Update Account
        public async Task<Result<AccountResponse>> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest)
        {
            var oldAccount = await _accountRepository.GetAccountByIdAsync(id);

            if (oldAccount == null)
            {
                return new Result<AccountResponse>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            var newAccount = _mapper.Map(updateAccountRequest, oldAccount);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new Result<AccountResponse>("success", "Cập Nhập Tài Khoản Thành Công!", null);
            }
            catch (Exception)
            {
                return new Result<AccountResponse>("error", 400, "Cập Nhập Tài Khoản Thất Bại!");
            }

        }

        //Update Account Status For Admin
        public async Task<Result<AccountResponse>> UpdateAccountStatusForAdminAsync(int id, string status)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result<AccountResponse>("error", 404, "Tài Khoản Không Tồn Tại!");
            }

            if (!_util.CheckAccountStatusForAdmin(account.Status, status))
            {
                return new Result<AccountResponse>("error", 400, "Trạng Thái Tài Khoản Trùng Lập!");
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

                return new Result<AccountResponse>("success", "Cập Nhập Trạng Thái Tài Khoản Thành Công!", null);
            }
            catch (Exception)
            {
                return new Result<AccountResponse>("error", 400, "Cập Nhập Trạng Thái Tài Khoản Thất Bại!");
            }
        }
    }

}