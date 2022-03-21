using FanDuelDemo.Models;
using FanDuelDemo.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FanDuelDemo.Services
{
    public interface IPlayerPositionsService
    {
        Task<bool> IsSportExist(int sportId);
        Task<bool> IsTeamExist(int sportName, int teamId);
        Task<bool> IsPlayerExist(int sportName, int teamName, int playerId);
        Task<bool> IsPlayerAlreadyExistsInPosition(PlayerPositionVM playerPositionVM);
        Task<bool> IsPositionDepthAlreadyTaken(PlayerPositionVM playerPositionVM);
        Task<bool> AddPlayerToDepthChart(PlayerPositionVM playerPositionVM);
        Task<PlayerVM> RemovePlayerFromDepthChart(BackupsInputVM vm);
        Task<List<PlayerVM>> GetBackups(BackupsInputVM getBackupsVM);
        Task<List<FullDepthChartDisplayVM>> GetFullDepthChart();
    }
}