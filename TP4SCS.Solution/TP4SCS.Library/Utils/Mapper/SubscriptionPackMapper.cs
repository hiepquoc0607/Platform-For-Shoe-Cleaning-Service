using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.SubscriptionPack;
using TP4SCS.Library.Models.Response.SubcriptionPack;

namespace TP4SCS.Library.Utils.Mapper
{
    public class SubscriptionPackMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SubscriptionPack, SubscriptionPackResponse>();

            config.NewConfig<SubscriptionPackRequest, SubscriptionPack>();
        }
    }
}
