using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Response.Auth;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);

        Task<ApiResponse<AuthResponse>> CustomerRegisterAsync(CreateAccountRequest createAccountRequest);

        Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshToken refeshToken);

        Task<ApiResponse<AuthResponse>> SendOTPAsync(RefreshToken refeshToken);

        Task<ApiResponse<AuthResponse>> SendVerificationEmailAsync(string email);

        Task<ApiResponse<AuthResponse>> VerifyEmailAsync(VerifyEmailRequest verifyEmailRequest);

        Task<ApiResponse<AuthResponse>> ResetPasswordAsync(ResetPasswordQuery resetPasswordQuery, ResetPasswordRequest resetPasswordRequest);

        Task<ApiResponse<AuthResponse>> RequestResetPasswordAsync(string email);
    }
}