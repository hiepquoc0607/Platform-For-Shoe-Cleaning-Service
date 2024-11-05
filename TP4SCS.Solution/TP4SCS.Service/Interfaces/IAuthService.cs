using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Response.Auth;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);

        Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshToken refeshToken);

        Task<ApiResponse<AuthResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
    }
}