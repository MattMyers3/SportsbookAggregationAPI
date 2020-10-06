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

        public ActionResult<IEnumerable<Game>> GetGames(DateTime start, DateTime? end, string? sport)
        {
            start = start.ToUniversalTime();
            end = end == null ? start.AddHours(24) : end.Value.ToUniversalTime(); //If there's no end date assume the caller wants a 24 hour period
            try
            {
                if (sport == null)
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date >= start.Date && r.TimeStamp <= end).ToList();
                else
                {
                    var sportId = context.SportRepository.Read().Single(r => r.Name == sport).SportId;
                    var teamsFromSports = context.TeamRepository.Read().Where(r => r.Sport.SportId == sportId);
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date >= start && r.TimeStamp.Date <= end && teamsFromSports.Any(t => t.TeamId == r.HomeTeamId)).ToList();
                }
            }
            catch (Exception ex)
            {
                //Add logging
                throw ex;
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