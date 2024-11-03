using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<Location>?> GetCityAsync()
        {
            return await _dbContext.Locations
                .AsNoTracking()
                .Select(c => new Location
                {
                    City = c.City
                })
                .Distinct()
                .OrderBy(c => c.City)
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>?> GetProvinceByWardAsync(string name)
        {
            return await _dbContext.Locations
                .AsQueryable()
                .AsNoTracking()
                .Where(p => EF.Functions.Collate(p.Ward, "Vietnamese_100_CI_AI_KS_WS_SC_UTF8").Contains(name))
                //.Where(p => string.Equals(p.Ward, name, StringComparer.OrdinalIgnoreCase))
                .Select(p => new Location
                {
                    Province = p.Province
                })
                .Distinct()
                .OrderBy(p => p.Province)
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>?> GetWardByCityAsync(string name)
        {
            return await _dbContext.Locations
                .AsNoTracking()
                .Where(w => EF.Functions
                .Collate(w.City, "SQL_Latin1_General_CP1_CI_AI")
                .Contains(name))
                .Select(w => new Location
                {
                    Ward = w.Ward
                })
                .Distinct()
                .OrderBy(w => w.Ward)
                .ToListAsync();
        }
    }
}
