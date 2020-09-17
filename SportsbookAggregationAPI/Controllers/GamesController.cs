using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ActionResult<IEnumerable<Game>> GetGames(int year, int month, int day, string? sport)
        {
            try
            {
                var date = new DateTime(year, month, day);
                if (sport == null)
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date == date.Date).ToList();
                else
                {
                    var sportId = context.SportRepository.Read().Single(r => r.Name == sport).SportId;
                    var teamsFromSports = context.TeamRepository.Read().Where(r => r.Sport.SportId == sportId);
                    return context.GameRepository.Read().Where(r => r.TimeStamp.Date == date.Date && teamsFromSports.Any(t => t.TeamId == r.HomeTeamId)).ToList();
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