using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<IEnumerable<Location>?> GetCityAsync();

        Task<IEnumerable<Location>?> GetWardByCityAsync(string name);

        Task<IEnumerable<Location>?> GetProvinceByWardAsync(string name);
    }
}
