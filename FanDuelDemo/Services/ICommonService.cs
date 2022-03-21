using System.Threading.Tasks;

namespace FanDuelDemo.Services
{
    public interface ICommonService
    {
        Task<bool> IsSportAlreadyExist(string sportName);
        Task<bool> IsTeamAlreadyExist(int sportId, string teamName);
        Task<bool> IsPlayerNumberNotUnique(int sportId, int teamId, string playerName, int playerNo);
        Task<int> SportsCount();
        Task<int> TeamsCount();
        Task<int> PlayersCount();
        Task<bool> IsValidSport(int sportId);
        Task<bool> IsValidTeam(int sportId);
    }
}