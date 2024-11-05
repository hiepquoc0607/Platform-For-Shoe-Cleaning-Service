using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.Auth;

namespace TP4SCS.Library.Utils.Mapper
{
    public class AccountMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateAccountRequest, Account>()
                .Map(dest => dest.PasswordHash, src => src.Password)
                .Map(dest => dest.ImageUrl, opt => string.Empty)
                .Map(dest => dest.IsVerified, opt => true)
                .Map(dest => dest.IsGoogle, opt => false)
                .Map(dest => dest.RefreshToken, opt => string.Empty)
                .Map(dest => dest.RefreshExpireTime, opt => DateTime.Now)
                .Map(dest => dest.Fcmtoken, opt => string.Empty)
                .Map(dest => dest.CreatedByOwnerId, opt => (int?)null)
                .Map(dest => dest.Status, opt => "ACTIVE");

            config.NewConfig<Account, UpdateAccountRequest>();

            config.NewConfig<Account, UpdateAccountPasswordRequest>();

            config.NewConfig<Account, AccountResponse>();

            config.NewConfig<AuthResponse, Account>();
        }
    }
}
