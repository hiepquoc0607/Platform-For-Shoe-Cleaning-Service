using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Request.BusinessProfile;
using TP4SCS.Library.Models.Response.BusinessProfile;
using TP4SCS.Library.Utils.StaticClass;

namespace TP4SCS.Library.Utils.Mapper
{
    public class BusinessMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BusinessProfile, BusinessResponse>();

            config.NewConfig<CreateBusinessRequest, BusinessProfile>()
                .Map(dest => dest.Phone, src => src.BusinessPhone)
                .Map(dest => dest.CitizenId, otp => string.Empty)
                .Map(dest => dest.BackCitizenImageUrl, otp => string.Empty)
                .Map(dest => dest.FrontCitizenImageUrl, otp => string.Empty)
                .Map(dest => dest.ImageUrl, otp => "https://firebasestorage.googleapis.com/v0/b/shoecarehub-4dca3.firebasestorage.app/o/images%2Fe03d0778-890e-4cae-ad06-647a48756770.jpg?alt=media&token=42f7ce37-8693-4e94-bb4d-6615e0cad830")
                .Map(dest => dest.Rating, otp => 0)
                .Map(dest => dest.Rank, otp => int.MaxValue)
                .Map(dest => dest.TotalOrder, otp => 0)
                .Map(dest => dest.PendingAmount, otp => 0)
                .Map(dest => dest.ProcessingAmount, otp => 0)
                .Map(dest => dest.FinishedAmount, otp => 0)
                .Map(dest => dest.CanceledAmount, otp => 0)
                .Map(dest => dest.ToTalServiceNum, otp => 0)
                .Map(dest => dest.CreatedDate, otp => DateOnly.FromDateTime(DateTime.Now))
                .Map(dest => dest.RegisteredTime, otp => DateTime.Now)
                .Map(dest => dest.ExpiredTime, otp => DateTime.Now)
                .Map(dest => dest.Status, otp => StatusConstants.EXPIRED);

            config.NewConfig<BusinessProfile, UpdateBusinessRequest>();

            config.NewConfig<BusinessProfile, UpdateBusinessStatisticRequest>();

            config.NewConfig<BusinessProfile, UpdateBusinessSubcriptionRequest>();

        }
    }
}
