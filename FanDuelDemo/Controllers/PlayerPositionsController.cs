using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanDuelDemo;
using FanDuelDemo.Models;
using FanDuelDemo.ViewModels;
using FanDuelDemo.Services;

namespace FanDuelDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerPositionsController : ControllerBase
    {
        private readonly IPlayerPositionsService _service;

        public PlayerPositionsController(IPlayerPositionsService service)
        {
            _service = service;
        }

        
        [HttpPost("addplayertodepthchart")]
        public async Task<ActionResult<bool>> AddPlayerToDepthChart(PlayerPositionVM playerPositionVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isSuccess = await _service.AddPlayerToDepthChart(playerPositionVM);

            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("removeplayertodepthchart")]
        public async Task<ActionResult<PlayerVM>> RemovePlayerToDepthChart(BackupsInputVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var position = await _service.RemovePlayerFromDepthChart(vm);
                return Ok(position);
            }
            catch(Exception ex)
            {
                return BadRequest("Unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("getbackups")]
        public async Task<ActionResult<IEnumerable<PlayerVM>>> GetBackups(BackupsInputVM getbackupsVM)
        {
            try
            {
                var backups = await _service.GetBackups(getbackupsVM);
                return Ok(backups);
            }
            catch (Exception ex)
            {
                return BadRequest("Unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("getfulldepthchart")]
        public async Task<ActionResult<IEnumerable<FullDepthChartDisplayVM>>> GetFullDepthChart()
        {
            try
            {
                var positions = await _service.GetFullDepthChart();
                return Ok(positions);
            }
            catch (Exception ex)
            {
                return BadRequest("Unexpected error occurred. Please try again later.");
            }
        }
    }
}
