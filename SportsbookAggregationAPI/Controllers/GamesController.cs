using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregation.Data.Models;
using SportsbookAggregationAPI.Data;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly Context context;

        public GamesController(Context context)
        {
            this.context = context;
        }

        public ActionResult<IEnumerable<Game>> GetGames(int startYear, int startMonth, int startDay, int? endYear, int? endMonth, int? endDay, string? sport)
        {
            try
            {
                var startDate = new DateTime(startYear, startMonth, startDay);
                if((endYear == null || endMonth == null || endDay == null) && sport == null)
                {
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date >= startDate.Date).ToList();
                }
                if(endYear == null || endMonth == null || endDay == null)
                {
                    var sportId = context.SportRepository.Read().Single(r => r.Name == sport).SportId;
                    var teamsFromSports = context.TeamRepository.Read().Where(r => r.Sport.SportId == sportId);
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date == startDate.Date && teamsFromSports.Any(t => t.TeamId == r.HomeTeamId)).ToList();
                }
                var endDate = new DateTime(endYear.Value, endMonth.Value, endDay.Value);
                if (sport == null)
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date >= startDate.Date && r.TimeStamp.Date <= endDate.Date).ToList();
                else
                {
                    var sportId = context.SportRepository.Read().Single(r => r.Name == sport).SportId;
                    var teamsFromSports = context.TeamRepository.Read().Where(r => r.Sport.SportId == sportId);
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date >= startDate.Date && r.TimeStamp.Date <= endDate.Date && teamsFromSports.Any(t => t.TeamId == r.HomeTeamId)).ToList();
                }
            }
            catch (Exception ex)
            {
                return context.GameRepository.Read().Where(r => r.TimeStamp >= DateTime.Now).ToList();
            }
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public ActionResult<Game> GetGame(Guid id)
        {
            var game = context.GameRepository.Read().FirstOrDefault(r => r.GameId == id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }
    }
}