using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Response.Account;

namespace TP4SCS.Library.Utils.Mapper
{
    public class AccountMapper : IRegister
    {
        //public AccountMapper()
        //{
        //    CreateMap<CreateAccountRequest, Account>()
        //        .ForMember(dest => dest.ExpiredTime, opt => opt.MapFrom(src => DateTime.UtcNow))
        //        .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
        //        .ForMember(dest => dest.Fcmtoken, opt => opt.MapFrom(src => string.Empty))
        //        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "ACTIVE"));

        //    CreateMap<UpdateAccountRequest, Account>();

        //    CreateMap<Account, AccountResponse>();

        //    CreateMap<AccountResponse, Account>();

        //    CreateMap<UpdateAccountRequest, AccountResponse>();
        //}
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateAccountRequest, Account>()
                .Map(dest => dest.PasswordHash, src => src.Password)
                .Map(dest => dest.ExpiredTime, opt => DateTime.Now)
                .Map(dest => dest.ImageUrl, opt => string.Empty)
                .Map(dest => dest.IsGoogle, opt => false)
                .Map(dest => dest.RefreshToken, opt => string.Empty)
                .Map(dest => dest.Fcmtoken, opt => string.Empty)
                .Map(dest => dest.Status, opt => "ACTIVE");

            config.NewConfig<Account, UpdateAccountRequest>()
                .Map(dest => dest.Password, src => src.PasswordHash);

            config.NewConfig<Account, AccountResponse>();
        }
    }
}
