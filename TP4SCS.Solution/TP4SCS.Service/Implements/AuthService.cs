using Azure.Core;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.Auth;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly Util _util;
        private static readonly DateTime _time = DateTime.Now.AddHours(1);

        public AuthService(IConfiguration configuration,
            IAccountRepository accountRepository,
            IBusinessRepository businessRepository,
            IBranchRepository branchRepository,
            IEmailService emailService,
            IMapper mapper,
            Util util)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _businessRepository = businessRepository;
            _branchRepository = branchRepository;
            _emailService = emailService;
            _mapper = mapper;
            _util = util;
        }

        private int CaculateSeccond(DateTime dateTime)
        {
            return (int)(dateTime - DateTime.Now).TotalSeconds;
        }

        private string GenerateToken(Account account)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: _time,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        //Login
        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var email = loginRequest.Email.Trim().ToLower();

            var account = await _accountRepository.GetAccountLoginByEmailAsync(email);

            if (account == null)
            {
                return new ApiResponse<AuthResponse>("error", 401, "Xác thực thất bại!");
            }

            if (account.Status.Equals("SUSPENDED"))
            {
                return new ApiResponse<AuthResponse>("error", 401, "Tài Khoản Đã Bị Khoá!");
            }

            if (!account.IsVerified)
            {
                return new ApiResponse<AuthResponse>("error", 401, "Email Tài Khoản Chưa Được Xác Nhận!");
            }

            if (!_util.CompareHashedPassword(loginRequest.Password, account.PasswordHash))
            {
                return new ApiResponse<AuthResponse>("error", 401, "Mật Khẩu Không Đúng!");
            }

            var token = GenerateToken(account);
            var refreshToken = GenerateRefreshToken();
            var expiredIn = CaculateSeccond(_time);

            account.RefreshToken = token;
            account.RefreshExpireTime = DateTime.UtcNow.AddDays(1);

            var data = _mapper.Map<AuthResponse>(account);
            data.Token = token;
            data.ExpiresIn = expiredIn;

            if (data.Role.Equals("OWNER", StringComparison.OrdinalIgnoreCase))
            {
                data.BusinessId = await _businessRepository.GetBusinessIdByOwnerIdAsync(account.Id);
            }

            if (data.Role.Equals("EMPLOYEE", StringComparison.OrdinalIgnoreCase))
            {
                data.BranchId = await _branchRepository.GetBranchIdByEmployeeIdAsync(account.Id);
            }

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new ApiResponse<AuthResponse>("success", "Đăng Nhập Thành Công!", data);
            }
            catch (Exception)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Đăng Nhập Thất Bại!");
            }
        }

        //Reset Password
        public async Task<ApiResponse<AuthResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var passwordError = _util.CheckPasswordErrorType(resetPasswordRequest.NewPassword);

            var passwordErrorMessages = new Dictionary<string, string>
            {
                { "Number", "Mật khẩu phải chứa ít nhất 1 kí tự số!" },
                { "Lower", "Mật khẩu phải chứa ít nhất 1 kí tự viết thường!" },
                { "Upper", "Mật khẩu phải chứa ít nhất 1 kí tự viết hoa!" },
                { "Special", "Mật khẩu phải chứa ít nhất 1 kí tự đặc biệt (!, @, #, $,...)!" },
            };

            if (passwordErrorMessages.TryGetValue(passwordError, out var message))
            {
                return new ApiResponse<AuthResponse>("error", 400, message);
            }

            var account = await _accountRepository.GetAccountByEmailAsync(resetPasswordRequest.Email.Trim());

            if (account == null)
            {
                return new ApiResponse<AuthResponse>("error", 404, "Email Không Tồn Tại!");
            }

            if (!resetPasswordRequest.NewPassword.Equals(resetPasswordRequest.ConfirmPassword))
            {
                return new ApiResponse<AuthResponse>("error", 400, "Mật Khẩu Xác Nhận Không Trùng!");
            }

            account.PasswordHash = _util.HashPassword(resetPasswordRequest.NewPassword);

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new ApiResponse<AuthResponse>("success", "Đặt Lại Mật Khẩu Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Đặt Lại Mật Khẩu Thất Bại!");
            }
        }

        public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshToken refeshToken)
        {
            var account = await _accountRepository.GetAccountByIdAsync(refeshToken.AccountId);

            if (account == null)
            {
                return new ApiResponse<AuthResponse>("error", 404, "Không Tìm Thấy Tài Khoản!");
            }

            if (account.RefreshExpireTime == null || account.RefreshExpireTime <= DateTime.Now)
            {
                return new ApiResponse<AuthResponse>("error", 401, "Hết Phiên Đăng Nhập!");
            }

            var token = GenerateToken(account);
            var newRefreshToken = GenerateRefreshToken();
            var expiredIn = CaculateSeccond(_time);

            account.RefreshToken = token;
            account.RefreshExpireTime = DateTime.UtcNow.AddDays(1);

            var data = _mapper.Map<AuthResponse>(account);
            data.Token = token;
            data.ExpiresIn = expiredIn;

            if (data.Role.Equals("OWNER", StringComparison.OrdinalIgnoreCase))
            {
                data.BusinessId = await _businessRepository.GetBusinessIdByOwnerIdAsync(account.Id);
            }

            if (data.Role.Equals("EMPLOYEE", StringComparison.OrdinalIgnoreCase))
            {
                data.BranchId = await _branchRepository.GetBranchIdByEmployeeIdAsync(account.Id);
            }

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new ApiResponse<AuthResponse>("success", "Tạo Mới Refresh Token Thành Công!", data);
            }
            catch (Exception)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Tạo Mới Refresh Token Thất Bại!");
            }
        }

        public Task<ApiResponse<AuthResponse>> SendOTPAsync(RefreshToken refeshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<AuthResponse>> VerifyEmailAsync(VerifyEmailRequest verifyEmailRequest)
        {
            var account = await _accountRepository.GetAccountByIdAsync(verifyEmailRequest.AccountId);

            if (account == null)
            {
                return new ApiResponse<AuthResponse>("error", 404, "Không Tìm Thấy Tài Khoản!");
            }

            if (account.RefreshToken != verifyEmailRequest.Token)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Xác Nhận Email Thất Bại!");
            }

            if (account.IsVerified)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Email Đã Được Xác Nhận!");
            }

            account.IsVerified = true;
            account.RefreshToken = string.Empty;
            account.RefreshExpireTime = null;

            try
            {
                await _accountRepository.UpdateAsync(account);

                return new ApiResponse<AuthResponse>("success", "Xác Nhận Email Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Xác Nhận Email Thất Bại!");
            }
        }

        public async Task<ApiResponse<AuthResponse>> SendVerificationEmailAsync(string email, string url)
        {
            try
            {
                await _emailService.SendEmailAsync(email, "ShoeCareHub Email Verification", url);

                return new ApiResponse<AuthResponse>("success", "Gửi Email Xác Nhận Thành Công!", null);
            }
            catch (Exception)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Gửi Email Xác Nhận Thất Bại!");
            }
        }

        public async Task<ApiResponse<AuthResponse>> CustomerRegisterAsync(CreateAccountRequest createAccountRequest)
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
                return new ApiResponse<AuthResponse>("error", 400, message);
            }

            var isEmailExisted = await _accountRepository.IsEmailExistedAsync(createAccountRequest.Email.Trim().ToLower());

            if (isEmailExisted == true)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Email đã được sử dụng!");
            }

            var isPhoneExisted = await _accountRepository.IsPhoneExistedAsync(createAccountRequest.Phone.Trim());

            if (isPhoneExisted == true)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Số điện thoại đã được sử dụng!");
            }

            var newAccount = _mapper.Map<Account>(createAccountRequest);
            newAccount.PasswordHash = _util.HashPassword(createAccountRequest.Password);
            newAccount.RefreshToken = GenerateRefreshToken();
            newAccount.Role = RoleConstants.CUSTOMER;

            try
            {
                await _accountRepository.CreateAccountAsync(newAccount);

                //var maxId = await _accountRepository.GetAccountMaxIdAsync();

                var newAcc = await _accountRepository.GetAccountByIdAsync(newAccount.Id);

                if (newAcc == null)
                {
                    return new ApiResponse<AuthResponse>("error", 400, "Tạo Tài Khoản Thất Bại!");
                }

                var data = _mapper.Map<AuthResponse>(newAcc);

                return new ApiResponse<AuthResponse>("success", "Tạo Tài Khoản Thành Công!", data);
            }
            catch (Exception)
            {
                return new ApiResponse<AuthResponse>("error", 400, "Tạo Tài Khoản Thất Bại!");
            }
        }
    }
}
