using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Response.Account;

namespace TP4SCS.Library.Utils.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<CreateAccountRequest, Account>()
                .ForMember(dest => dest.ExpiredTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.IsGoogle, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Fcmtoken, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "ACTIVE"));

            CreateMap<UpdateAccountRequest, Account>();

            CreateMap<Account, AccountResponse>();

            CreateMap<AccountResponse, Account>();

            CreateMap<UpdateAccountRequest, AccountResponse>();
        }
    }
}
