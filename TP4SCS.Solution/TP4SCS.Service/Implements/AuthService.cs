using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Response.Auth;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly Util _util;
        private static readonly DateTime _time = DateTime.Now.AddDays(1);

        public AuthService(IConfiguration configuration, IAccountRepository accountRepository, Util util)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
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
        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var account = await _accountRepository.GetAccountLoginByEmailAsync(loginRequest.Email);

            if (account == null)
            {
                return new Result<AuthResponse>("error", 404, "Email Không Tồn Tại!");
            }

            if (!_util.CompareHashedPassword(loginRequest.Password, account.PasswordHash))
            {
                return new Result<AuthResponse>("error", 400, "Mật Khẩu Không Đúng!");
            }

            var token = GenerateToken(account);
            var expiredIn = CaculateSeccond(_time);

            var data = new AuthResponse
            {
                Id = account.Id,
                Email = account.Email,
                Fullname = account.FullName,
                Phone = account.Phone,
                Gender = account.Gender,
                Dob = account.Dob,
                ImageUrl = account.ImageUrl,
                RefreshToken = account.RefreshToken,
                Fcmtoken = account.Fcmtoken,
                Role = account.Role,
                Token = token,
                ExpiresIn = expiredIn
            };

            return new Result<AuthResponse>("success", "Đăng Nhập Thành Công!", data);
        }

        //Reset Password
        public async Task<Result<AuthResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(resetPasswordRequest.Email);

            if (account == null)
            {
                return new Result<AuthResponse>("error", 404, "Email Không Tồn Tại!");
            }

            if (!resetPasswordRequest.NewPassword.Equals(resetPasswordRequest.ConfirmPassword))
            {
                return new Result<AuthResponse>("error", 400, "Mật Khẩu Xác Nhận Không Trùng!");
            }

            account.PasswordHash = _util.HashPassword(resetPasswordRequest.NewPassword);

            try
            {
                await _accountRepository.UpdateAccountAsync(account);

                return new Result<AuthResponse>("success", "Đặt Lại Mật Khẩu Thành Công!", null);
            }
            catch (Exception)
            {
                return new Result<AuthResponse>("error", 400, "Đặt Lại Mật Khẩu Thất Bại!");
            }
        }
    }
}
