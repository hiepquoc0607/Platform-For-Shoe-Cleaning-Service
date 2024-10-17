using AutoMapper;
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

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IAuthService authService, Util util)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _authService = authService;
            _util = util;
        }

        public async Task<Result> CreateAccountAsync(CreateAccountRequest createAccountRequest, int role)
        {
            var email = _util.LowerCaseString(createAccountRequest.Email);
            var passwordError = _util.CheckPasswordErrorType(createAccountRequest.Password);

            var passwordErrorMessages = new Dictionary<string, string>
            {
                { "Length", "Password must be at least 8 characters!" },
                { "Number", "Password must contain at least 1 number character!" },
                { "Lower", "Password must contain at least 1 lowercase character!" },
                { "Upper", "Password must contain at least 1 uppercase character!" },
                { "Special", "Password must contain at least 1 special character!" },
            };

            if (passwordErrorMessages.TryGetValue(passwordError, out var message))
            {
                return new Result { IsSuccess = false, Message = message };
            }

            var isEmailExisted = await _accountRepository.IsEmailExistedAsync(email);

            if (isEmailExisted == true)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Email Has Already Been Registered!" };
            }

            var isPhoneExisted = await _accountRepository.IsPhoneExistedAsync(createAccountRequest.Phone);

            if (isPhoneExisted == true)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Phone Has Already Been Registered!" };
            }

            var newAccount = _mapper.Map<Account>(createAccountRequest);

            newAccount.Role = role switch
            {
                1 => "OWNER",
                2 => "EMPLOYEE",
                3 => "MODERATOR",
                _ => "CUSTOMER"
            };

            newAccount.PasswordHash = _util.HashPassword(createAccountRequest.Password);

            try
            {
                await _accountRepository.InsertAsync(newAccount);

                return new Result { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Created Fail!" };
            }
        }

        public async Task<Result> DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 404, Message = "Account Does Not Exist!" };
            }

            account.Status = "INACTIVE";

            var newAccount = _mapper.Map<Account>(account);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new Result { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Deleted Fail!" };
            }

        }

        public async Task<AccountResponse?> GetAccountByIdAsync(int id)
        {
            var result = await _accountRepository.GetAccountByIdAsync(id);

            if (result == null)
            {
                return null;
            }

            result.Role = _util.TranslateAccountRole(result.Role);
            result.Status = _util.TranslateAccountStatus(result.Status);

            return result;
        }

        public async Task<int> GetAccountMaxIdAsync()
        {
            return await _accountRepository.GetAccountMaxIdAsync();
        }

        public async Task<IEnumerable<AccountResponse>?> GetAccountsAsync(GetAccountRequest getAccountRequest)
        {
            var result = await _accountRepository.GetAccountsAsync();

            if (result == null)
            {
                return null;
            }

            //Search
            if (!string.IsNullOrEmpty(getAccountRequest.SearchKey))
            {
                result = result.Where(r => r.FullName.Contains(getAccountRequest.SearchKey) || r.Email.Contains(getAccountRequest.SearchKey));
            }

            //Sort
            if (getAccountRequest.IsDecsending == true)
            {
                result = getAccountRequest.SortBy switch
                {
                    SortOption.Email => result.OrderByDescending(r => r.Email),
                    SortOption.Fullname => result.OrderByDescending(r => r.FullName),
                    _ => result
                };
            }

            if (getAccountRequest.IsDecsending == false)
            {
                result = getAccountRequest.SortBy switch
                {
                    SortOption.Email => result.OrderBy(r => r.Email),
                    SortOption.Fullname => result.OrderBy(r => r.FullName),
                    _ => result
                };
            }

            //Paging
            int skipNum = (getAccountRequest.PageNum - 1) * getAccountRequest.PageSize;

            return result.Skip(skipNum).Take(getAccountRequest.PageSize).ToList();
        }

        public async Task<Result> LoginAsync(LoginRequest loginRequest)
        {
            var account = await _accountRepository.GetAccountLoginByEmailAsync(loginRequest.Email);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Email Not Found!" };
            }

            if (!_util.CompareHashedPassword(loginRequest.Password, account.PasswordHash))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Password Is Incorrect!" };
            }

            var token = _authService.GenerateToken(account);

            return new Result { IsSuccess = true, Token = token };
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(resetPasswordRequest.Email);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Email Not Found!" };
            }

            if (!resetPasswordRequest.NewPassword.Equals(resetPasswordRequest.ConfirmPassword))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Password Not Match!" };
            }

            account.Password = _util.HashPassword(resetPasswordRequest.NewPassword);

            var newAccount = _mapper.Map<Account>(account);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new Result { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Password Updated Fail!" };
            }
        }

        public async Task<Result> UpdateAccountAsync(int id, UpdateAccountRequest updateAccountRequest)
        {
            var oldAccount = await _accountRepository.GetAccountByIdAsync(id);

            if (oldAccount == null)
            {
                return new Result { IsSuccess = false, StatusCode = 404, Message = "Account Does Not Exist!" };
            }

            var mapAccount = _mapper.Map(updateAccountRequest, oldAccount);

            var newAccount = _mapper.Map<Account>(mapAccount);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new Result { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Updated Fail!" };
            }

        }

        public async Task<Result> UpdateAccountStatusForAdminAsync(int id, StatusAdminRequest status)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return new Result { IsSuccess = false, StatusCode = 404, Message = "Account Does Not Exist!" };
            }

            if (!_util.CheckAccountStatusForAdmin(account.Status, status))
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Status Is At This State" };
            }

            account.Status = status switch
            {
                StatusAdminRequest.INACTIVE => "INACTIVE",
                StatusAdminRequest.SUSPENDED => "SUSPENDED",
                _ => "ACTIVE"
            };

            var newAccount = _mapper.Map<Account>(account);

            try
            {
                await _accountRepository.UpdateAccountAsync(newAccount);

                return new Result { IsSuccess = true };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, StatusCode = 400, Message = "Account Status Updated Fail!" };
            }
        }
    }

}