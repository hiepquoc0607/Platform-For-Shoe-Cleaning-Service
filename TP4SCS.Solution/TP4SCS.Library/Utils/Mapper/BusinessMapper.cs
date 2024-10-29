using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Business;
using TP4SCS.Library.Models.Request.BusinessProfile;
using TP4SCS.Library.Models.Response.BusinessProfile;

namespace TP4SCS.Library.Utils.Mapper
{
    public class BusinessMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BusinessProfile, BusinessResponse>();

            config.NewConfig<CreateBusinessObject, BusinessProfile>()
                .Map(dest => dest.Rating, otp => 0)
                .Map(dest => dest.Rank, otp => int.MaxValue)
                .Map(dest => dest.TotalOrder, otp => 0)
                .Map(dest => dest.PendingAmount, otp => 0)
                .Map(dest => dest.ProcessingAmount, otp => 0)
                .Map(dest => dest.FinishedAmount, otp => 0)
                .Map(dest => dest.CanceledAmount, otp => 0)
                .Map(dest => dest.Status, otp => "ACTIVE");

            config.NewConfig<BusinessProfile, UpdateBusinessRequest>();

            config.NewConfig<BusinessProfile, UpdateBusinessStatisticRequest>();

            config.NewConfig<BusinessProfile, UpdateBusinessSubcriptionRequest>();

        }
    }
}
