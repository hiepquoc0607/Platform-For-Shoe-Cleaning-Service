using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Response.Location;

namespace TP4SCS.Library.Utils.Mapper
{
    public class LocationMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Location, LocationResponse>();
        }
    }
}
