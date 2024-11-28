using TP4SCS.Library.Models.Data;

namespace TP4SCS.Repository.Interfaces
{
    public interface ILeaderboardRepository : IGenericRepository<Leaderboard>
    {
        Task<IEnumerable<Leaderboard>?> GetLeaderboardByWeekAsync();

        Task<IEnumerable<Leaderboard>?> GetLeaderboardByMonthAsync();

        Task<IEnumerable<Leaderboard>?> GetLeaderboardByYearAsync();

        Task CreateLeaderboardAsync(Leaderboard leaderboard);
    }
}
