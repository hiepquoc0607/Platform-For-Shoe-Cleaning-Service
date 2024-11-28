using TP4SCS.Library.Models.Data;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class LeaderboardRepository : GenericRepository<Leaderboard>, ILeaderboardRepository
    {
        public LeaderboardRepository(Tp4scsDevDatabaseContext dbContext) : base(dbContext)
        {
        }

        public Task CreateLeaderboardAsync(Leaderboard leaderboard)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Leaderboard>?> GetLeaderboardByMonthAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Leaderboard>?> GetLeaderboardByWeekAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Leaderboard>?> GetLeaderboardByYearAsync()
        {
            throw new NotImplementedException();
        }
    }
}
