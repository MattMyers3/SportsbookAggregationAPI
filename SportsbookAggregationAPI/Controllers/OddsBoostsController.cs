using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.AggregationModels;
using SportsbookAggregationAPI.Services;
using SportsbookAggregationAPI.SportsbookModels;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OddsBoostsController : ControllerBase
    {
        private readonly Context context;

        public OddsBoostsController(Context context)
        {
            this.context = context;
        }

        // GET: api/OddsBoosts
        [HttpGet]
        public ActionResult<IEnumerable<OddsBoostWithSportsbook>> GetOddsBoost()
        {
            var oddsBoosts = context.OddsBoostRepository.Read().Where(o => o.IsAvailable).ToList();

            var oddsBoostsWithSportsbooks = new List<OddsBoostWithSportsbook>();
            var gamblingSites = context.GamblingSiteRepository.Read().ToList();
            foreach (var boost in oddsBoosts)
            {
                var gamblingSiteName = gamblingSites.First(s => s.GamblingSiteId == boost.GamblingSiteId).Name;
                var boostWithSportsbook = new OddsBoostWithSportsbook
                {
                    BoostedOdds = boost.BoostedOdds,
                    Date = boost.Date,
                    Description = boost.Description,
                    OddsBoostId = boost.OddsBoostId,
                    PreviousOdds = boost.PreviousOdds,
                    SiteName = gamblingSiteName
                };
                oddsBoostsWithSportsbooks.Add(boostWithSportsbook);
            }
            return oddsBoostsWithSportsbooks;
        }

        [Authorize]
        [HttpPut]
        public HttpStatusCode Update(OddsBoostUpdateObject oddsBoostUpdateObject)
        {
            if (HttpContext.User.Claims.Single(c => c.Type == "cid")?.Value != "0oa60prueRe8fdEkB5d6") //Aggregator account
                return HttpStatusCode.Unauthorized;

            using (var dbContext = new Context())
            {
                var oddsBoostService = new OddsBoostService(dbContext);
                using (var dbContextTransaction = dbContext.Database.BeginTransaction())
                {
                    oddsBoostService.Update(oddsBoostUpdateObject);
                    dbContextTransaction.Commit();
                }
            }
            return HttpStatusCode.NoContent;
        }

        [HttpGet("LastRefreshTime")]
        public ActionResult<LastRefresh> GetLastRefreshTime()
        {
            var lastRefreshTime = context.OddsBoostRepository.Read().Max(l => l.LastRefresh);

            return new LastRefresh { LastRefreshTime = lastRefreshTime };
        }
    }
}
