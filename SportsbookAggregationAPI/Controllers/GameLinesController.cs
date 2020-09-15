using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsbookAggregation.Data.Models;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.SportsbookModels;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameLinesController : ControllerBase
    {
        private readonly Context context;

        public GameLinesController(Context context)
        {
            this.context = context;
        }

        // GET: api/GameLines/5
        [HttpGet("{id}")]
        public ActionResult<GameLine> GetGameLine(Guid id)
        {
            var gameLine = context.GameLineRepository.Read().FirstOrDefault(r => r.GameLineId == id);

            if (gameLine == null)
            {
                return NotFound();
            }

            return gameLine;
        }

        // PUT: api/GameLines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameLine(Guid id, GameLine gameLine)
        {
            if (id != gameLine.GameLineId)
            {
                return BadRequest();
            }

            context.Entry(gameLine).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.GameLineRepository.Read().Any(r => r.GameLineId == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/GameLines
        [HttpPost]
        public ActionResult<GameLine> PostGameLine(GameLine gameLine)
        {
            context.GameLineRepository.Create(gameLine);

            return CreatedAtAction("GetGameLine", new {gameLineId = gameLine.GameLineId}, gameLine);
        }

        // DELETE: api/GameLines/5
        [HttpDelete("{id}")]
        public ActionResult<GameLine> DeleteGameLine(Guid id)
        {
            var gameLine = context.GameLineRepository.Read().FirstOrDefault(r => r.GameLineId == id);
            if (gameLine == null)
            {
                return NotFound();
            }

            context.GameLineRepository.Delete(gameLine);
            return gameLine;
        }

        [HttpGet("best/{id}")]
        public ActionResult<BestAvailableGameLine> GetBestAvailableGameLine(Guid id)
        {
            var availableGameLines = context.GameLineRepository.Read().Where(r => r.GameId == id).ToList();
            if (!availableGameLines.Any())
                return NotFound();

            var gamblingSites = context.GamblingSiteRepository.Read().ToList();
            var bestAvailableGameLine = new BestAvailableGameLine();
            foreach (var availableGameLine in availableGameLines)
            {
                var gamblingSiteName = gamblingSites.First(s => s.GamblingSiteId == availableGameLine.GamblingSiteId).Name;
                if(bestAvailableGameLine.CurrentHomeSpread == availableGameLine.CurrentSpread && 
                    bestAvailableGameLine.CurrentHomeSpreadPayout < availableGameLine.HomeSpreadPayout)
                {
                    bestAvailableGameLine.CurrentHomeSpreadPayout = availableGameLine.HomeSpreadPayout.Value;
                    bestAvailableGameLine.HomeSpreadSite = gamblingSiteName;
                }
                else if (bestAvailableGameLine.CurrentHomeSpread == null ||
                    bestAvailableGameLine.CurrentHomeSpread < availableGameLine.CurrentSpread)
                {
                    bestAvailableGameLine.CurrentHomeSpreadPayout = availableGameLine.HomeSpreadPayout.Value;
                    bestAvailableGameLine.CurrentHomeSpread = availableGameLine.CurrentSpread;
                    bestAvailableGameLine.HomeSpreadSite = gamblingSiteName;
                }

                var currentAwaySpread = availableGameLine.CurrentSpread * -1;
                if (bestAvailableGameLine.CurrentAwaySpread == currentAwaySpread &&
                    bestAvailableGameLine.CurrentAwaySpreadPayout < availableGameLine.AwaySpreadPayout)
                {
                    bestAvailableGameLine.CurrentAwaySpreadPayout = availableGameLine.AwaySpreadPayout.Value;
                    bestAvailableGameLine.AwaySpreadSite = gamblingSiteName;
                }
                else if (bestAvailableGameLine.CurrentAwaySpread == null ||
                    bestAvailableGameLine.CurrentAwaySpread < currentAwaySpread)
                {
                    bestAvailableGameLine.CurrentHomeSpreadPayout = availableGameLine.AwaySpreadPayout.Value;
                    bestAvailableGameLine.CurrentAwaySpread = currentAwaySpread;
                    bestAvailableGameLine.AwaySpreadSite = gamblingSiteName;
                }

                if (bestAvailableGameLine.CurrentOver == null ||
                    bestAvailableGameLine.CurrentOver > availableGameLine.CurrentOverUnder)
                {
                    bestAvailableGameLine.CurrentOver = availableGameLine.CurrentOverUnder;
                    bestAvailableGameLine.OverSite = gamblingSiteName;
                }

                if (bestAvailableGameLine.CurrentUnder == null ||
                    bestAvailableGameLine.CurrentUnder < availableGameLine.CurrentOverUnder)
                {
                    bestAvailableGameLine.CurrentUnder = availableGameLine.CurrentOverUnder;
                    bestAvailableGameLine.UnderSite = gamblingSiteName;
                }

                if (bestAvailableGameLine.CurrentHomeMoneyLine == null || bestAvailableGameLine.CurrentHomeMoneyLine <
                    availableGameLine.HomeMoneyLinePayout)
                {
                    bestAvailableGameLine.CurrentHomeMoneyLine = availableGameLine.HomeMoneyLinePayout;
                    bestAvailableGameLine.HomeMoneyLineSite = gamblingSiteName;
                }
                
                if (bestAvailableGameLine.CurrentAwayMoneyLine == null || bestAvailableGameLine.CurrentAwayMoneyLine <
                    availableGameLine.AwayMoneyLinePayout)
                {
                    bestAvailableGameLine.CurrentAwayMoneyLine = availableGameLine.AwayMoneyLinePayout;
                    bestAvailableGameLine.AwayMoneyLineSite = gamblingSiteName;
                }
            }

            return bestAvailableGameLine;
        }

        [HttpGet("LastRefreshTime")]
        public ActionResult<LastRefresh> GetLastRefreshTime(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var gamesIds = context.GameRepository.Read().Where(r => r.TimeStamp.Date == date.Date).Select(g => g.GameId).ToList();
            return new LastRefresh { LastRefreshTime = context.GameLineRepository.Read().Where(r => gamesIds.Contains(r.GameId)).Max(l => l.LastRefresh) };
        }
    }
}