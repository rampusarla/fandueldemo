using FanDuelDemo.Models;
using FanDuelDemo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanDuelDemo.Services
{
    public class CommonService : ICommonService
    {
        private readonly FanDuelDbContext _context;

        public CommonService(FanDuelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSportAlreadyExist(string sportName)
        {
            return await _context.Sports.AnyAsync(s => s.Name.ToUpper() == sportName.ToUpper());
        }

        public async Task<bool> IsTeamAlreadyExist(int sportId, string teamName)
        {
            return await _context.Teams.AnyAsync(t => t.Name.ToUpper() == teamName.ToUpper() &&
                                        t.SportId == sportId);
        }

        public async Task<bool> IsValidSport(int sportId)
        {
            return await _context.Sports.AnyAsync(s => s.Id == sportId);
        }

        public async Task<bool> IsValidTeam(int teamId)
        {
            return await _context.Teams.AnyAsync(s => s.Id == teamId);
        }

        public async Task<int> SportsCount()
        {
            return await _context.Sports.CountAsync();
        }
        public async Task<int> TeamsCount()
        {
            return await _context.Sports.CountAsync();
        }
        public async Task<int> PlayersCount()
        {
            return await _context.Sports.CountAsync();
        }

        public async Task<bool> IsPlayerNumberNotUnique(int sportId, int teamId, string playerName, int playerNo)
        {
            return await _context.Players.AnyAsync(p => p.SportId == sportId &&
                                        p.TeamId == teamId &&
                                        p.Name.ToUpper() == playerName.ToUpper() && 
                                        p.Number == playerNo);
        }
    }
}
