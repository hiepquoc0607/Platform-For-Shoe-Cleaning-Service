using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Response.Auth;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly Util _util;
        private static readonly DateTime _time = DateTime.Now.AddDays(1);

        public AuthService(IConfiguration configuration, IAccountRepository accountRepository, IMapper mapper, Util util)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
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

            if (!_util.CompareHashedPassword(loginRequest.Password, account.PasswordHash))
            {
                return new ApiResponse<AuthResponse>("error", 401, "Mật Khẩu Không Đúng!");
            }

            var token = GenerateToken(account);
            var expiredIn = CaculateSeccond(_time);

            var data = _mapper.Map<AuthResponse>(account);
            data.Token = token;
            data.ExpiresIn = expiredIn;

            return new ApiResponse<AuthResponse>("success", "Đăng Nhập Thành Công!", data);
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
    }
}
