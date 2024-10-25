using TP4SCS.Library.Models.Request.Auth;
using TP4SCS.Library.Models.Response.Auth;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest);

        Task<Result<AuthResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
    }
}