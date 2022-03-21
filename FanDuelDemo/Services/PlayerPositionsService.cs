using AutoMapper;
using FanDuelDemo.Models;
using FanDuelDemo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanDuelDemo.Services
{
    public class PlayerPositionsService : IPlayerPositionsService
    {
        private readonly FanDuelDbContext _context;

        public PlayerPositionsService(FanDuelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSportExist(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }

        public async Task<bool> IsTeamExist(int sportId, int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId &&
                                        t.SportId == sportId);
        }

        public async Task<bool> IsPlayerExist(int sportId, int teamId, int playerId)
        {
            return await _context.Players.AnyAsync(p => p.SportId == sportId &&
                                        p.TeamId == teamId &&
                                        p.Id == playerId);
        }

        public async Task<bool> IsPlayerAlreadyExistsInPosition(PlayerPositionVM playerPositionVM)
        {
            return await _context.Positions
                    .AnyAsync(pos => pos.PositionName.ToUpper() == playerPositionVM.PositionName.ToUpper() &&
                    pos.PlayerId == playerPositionVM.PlayerId &&
                    pos.TeamId == playerPositionVM.TeamId &&
                    pos.SportId == playerPositionVM.SportId);
        }

        public async Task<bool> IsPositionDepthAlreadyTaken(PlayerPositionVM playerPositionVM)
        {
            if (playerPositionVM.PositionDepth.HasValue) {
                return await _context.Positions
                    .AnyAsync(pos => pos.PositionName.ToUpper() == playerPositionVM.PositionName.ToUpper() &&
                    pos.PositionDepth == playerPositionVM.PositionDepth.Value &&
                    pos.TeamId == playerPositionVM.TeamId &&
                    pos.SportId == playerPositionVM.SportId);
            }
            return false;
        }
        
        public async Task<bool> AddPlayerToDepthChart(PlayerPositionVM playerPositionVM)
        {
            bool isSuccess = true;
            try
            {
                var sport = await _context.Sports.SingleOrDefaultAsync(s => s.Id == playerPositionVM.SportId);
                var team = await _context.Teams.SingleOrDefaultAsync(t => t.SportId == sport.Id && t.Id == playerPositionVM.TeamId);
                var player = await _context.Players.SingleOrDefaultAsync(p => p.SportId == sport.Id &&
                                            p.TeamId == team.Id &&
                                            p.Id == playerPositionVM.PlayerId);
                int positionDepth;
                if (!playerPositionVM.PositionDepth.HasValue)
                {
                    var positions = _context.Positions.Where(p => p.SportId == sport.Id &&
                                            p.TeamId == team.Id &&
                                            p.PositionName.ToUpper() == playerPositionVM.PositionName.ToUpper());
                    if (positions == null)
                    {
                        positionDepth = 0;
                    }
                    else
                    {
                        positionDepth = positions.ToList().Max(p => p.PositionDepth) + 1;
                    }
                }
                else
                {
                    positionDepth = playerPositionVM.PositionDepth.Value;
                }

                var newPosition = new Position()
                {
                    PositionName = playerPositionVM.PositionName,
                    PlayerId = player.Id,
                    PositionDepth = positionDepth,
                    SportId = player.SportId,
                    TeamId = player.TeamId
                };

                _context.Positions.Add(newPosition);
                await _context.SaveChangesAsync();

                return isSuccess;

            }
            catch(Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
        }

        public async Task<PlayerVM> RemovePlayerFromDepthChart(BackupsInputVM vm)
        {
            try
            {
                var position = _context
                               .Positions
                               .SingleOrDefault(p => p.SportId == vm.SportId &&
                                                     p.TeamId == vm.TeamId &&
                                                     p.PlayerId == vm.PlayerId &&
                                                     p.PositionName.ToUpper() == vm.PositionName.ToUpper());
                if(position == null)
                {
                    return null;
                }
                else
                {
                    _context.Positions.Remove(position);
                    await _context.SaveChangesAsync();
                    return new PlayerVM() { 
                        PlayerId = vm.PlayerId,
                        PlayerName = _context.Players.SingleOrDefault(pl => pl.Id == vm.PlayerId).Name
                    };
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PlayerVM>> GetBackups(BackupsInputVM getBackupsVM)
        {
            try
            {
                var positions = _context
                               .Positions
                               .Where(p => p.SportId == getBackupsVM.SportId &&
                                                     p.TeamId == getBackupsVM.TeamId &&
                                                     p.PlayerId != getBackupsVM.PlayerId &&
                                                     p.PositionName.ToUpper() == getBackupsVM.PositionName.ToUpper());
                if (positions == null)
                {
                    return null;
                }
                else
                {
                    var backups = await positions.ToListAsync();
                    return backups.Select(p => new PlayerVM()
                    {
                        PlayerId = p.PlayerId,
                        PlayerName = _context.Players.SingleOrDefault(pl => pl.Id == p.PlayerId).Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FullDepthChartDisplayVM>> GetFullDepthChart()
        {
            try
            {
                var positions = _context.Positions;
                if (positions == null)
                {
                    return null;
                }
                else
                {
                    var fullDepthChart = new List<FullDepthChartDisplayVM>();
                    var players = new List<PlayerVM>();
                    foreach(var position in await positions.Select(p => p.PositionName).Distinct().ToListAsync())
                    {
                        players = positions.Where(p => p.PositionName == position).Select(p=>p)
                                        .Select(x => new PlayerVM()
                                        {
                                            PlayerId = x.PlayerId,
                                            PlayerName = _context.Players.SingleOrDefault(pl => pl.Id == x.PlayerId).Name
                                        }).ToList();

                        fullDepthChart.Add(new FullDepthChartDisplayVM()
                        {
                            PositionName = position,
                            Players = players
                        });
                    }
                    return fullDepthChart;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
