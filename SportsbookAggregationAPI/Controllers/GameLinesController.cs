using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.AggregationModels;
using SportsbookAggregationAPI.Data.DbModels;
using SportsbookAggregationAPI.Services;
using SportsbookAggregationAPI.SportsbookModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

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


        [HttpGet("best/{id}")]
        public ActionResult<BestAvailableGameLine> GetBestAvailableGameLine(Guid id, [FromQuery] string sportsbooks)
        {
            var bestAvailableGameLine = new BestAvailableGameLine();

            if (sportsbooks == null)
                return NotFound();
            var sportsbooksArray = sportsbooks?.Split(',');
            var availableGameLines = context.GameLineRepository.Read().Where(r => r.GameId == id && r.IsAvailable);
            if (!availableGameLines.Any())
                return NotFound();

            var gamblingSites = context.GamblingSiteRepository.Read().ToList();
            foreach (var availableGameLine in availableGameLines)
            {
                var gamblingSiteName = gamblingSites.First(s => s.GamblingSiteId == availableGameLine.GamblingSiteId).Name;
                if (sportsbooksArray != null && !sportsbooksArray.Contains(gamblingSiteName))
                    continue;
                if(bestAvailableGameLine.CurrentHomeSpread == availableGameLine.CurrentSpread && 
                    bestAvailableGameLine.CurrentHomeSpreadPayout < availableGameLine.HomeSpreadPayout)
                {
                    bestAvailableGameLine.CurrentHomeSpreadPayout = availableGameLine.HomeSpreadPayout;
                    bestAvailableGameLine.HomeSpreadSite = gamblingSiteName;
                }
                else if (bestAvailableGameLine.CurrentHomeSpread == null ||
                    bestAvailableGameLine.CurrentHomeSpread < availableGameLine.CurrentSpread)
                {
                    bestAvailableGameLine.CurrentHomeSpreadPayout = availableGameLine.HomeSpreadPayout;
                    bestAvailableGameLine.CurrentHomeSpread = availableGameLine.CurrentSpread;
                    bestAvailableGameLine.HomeSpreadSite = gamblingSiteName;
                }

                var currentAwaySpread = availableGameLine.CurrentSpread * -1;
                if (bestAvailableGameLine.CurrentAwaySpread == currentAwaySpread &&
                    bestAvailableGameLine.CurrentAwaySpreadPayout < availableGameLine.AwaySpreadPayout)
                {
                    bestAvailableGameLine.CurrentAwaySpreadPayout = availableGameLine.AwaySpreadPayout;
                    bestAvailableGameLine.AwaySpreadSite = gamblingSiteName;
                }
                else if (bestAvailableGameLine.CurrentAwaySpread == null ||
                    bestAvailableGameLine.CurrentAwaySpread < currentAwaySpread)
                {
                    bestAvailableGameLine.CurrentAwaySpreadPayout = availableGameLine.AwaySpreadPayout;
                    bestAvailableGameLine.CurrentAwaySpread = currentAwaySpread;
                    bestAvailableGameLine.AwaySpreadSite = gamblingSiteName;
                }

                if (bestAvailableGameLine.CurrentOver == availableGameLine.CurrentOverUnder &&
                    bestAvailableGameLine.CurrentOverPayout < availableGameLine.OverPayOut)
                {
                    bestAvailableGameLine.CurrentOverPayout = availableGameLine.OverPayOut;
                    bestAvailableGameLine.OverSite = gamblingSiteName;
                }
                else if (bestAvailableGameLine.CurrentOver == null ||
                    bestAvailableGameLine.CurrentOver > availableGameLine.CurrentOverUnder)
                {
                    bestAvailableGameLine.CurrentOver = availableGameLine.CurrentOverUnder;
                    bestAvailableGameLine.CurrentOverPayout = availableGameLine.OverPayOut;
                    bestAvailableGameLine.OverSite = gamblingSiteName;
                }

                if (bestAvailableGameLine.CurrentUnder == availableGameLine.CurrentOverUnder &&
                    bestAvailableGameLine.CurrentUnderPayout < availableGameLine.UnderPayout)
                {
                    bestAvailableGameLine.CurrentUnderPayout = availableGameLine.UnderPayout;
                    bestAvailableGameLine.UnderSite = gamblingSiteName;
                }
                else if (bestAvailableGameLine.CurrentUnder == null ||
                    bestAvailableGameLine.CurrentUnder < availableGameLine.CurrentOverUnder)
                {
                    bestAvailableGameLine.CurrentUnder = availableGameLine.CurrentOverUnder;
                    bestAvailableGameLine.CurrentUnderPayout = availableGameLine.UnderPayout;
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
        
        [Authorize]
        [HttpPut]
        public HttpStatusCode Update(GameOffering[] gameOfferings)
        {
            if (HttpContext.User.Claims.Single(c => c.Type == "cid")?.Value != "0oa60prueRe8fdEkB5d6") //Aggregator account
                return HttpStatusCode.Unauthorized;

            using (var dbContext = new Context())
            {
                var gameLineService = new GameLineService(dbContext);
                using (var dbContextTransaction = dbContext.Database.BeginTransaction())
                {
                    gameLineService.Update(gameOfferings);
                    dbContextTransaction.Commit();
                }
            }
            return HttpStatusCode.NoContent;
        }

        [HttpGet("LastRefreshTime")]
        public ActionResult<LastRefresh> GetLastRefreshTime()
        {
            try
            {
                var lastRefreshTime = context.GameLineRepository.Read().Max(l => l.LastRefresh);

                return new LastRefresh { LastRefreshTime = lastRefreshTime };
            }
            catch(Exception ex)
            {
                string filePath = "ErrorLog.txt";

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine();

                    var exception = ex;
                    while (exception != null)
                    {
                        writer.WriteLine(exception.GetType().FullName);
                        writer.WriteLine("Message : " + exception.Message);
                        writer.WriteLine("StackTrace : " + exception.StackTrace);

                        exception = exception.InnerException;
                    }
                }
                throw ex;
            }
            
        }
    }
}