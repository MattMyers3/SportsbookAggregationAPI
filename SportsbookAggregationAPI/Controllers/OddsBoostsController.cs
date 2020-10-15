using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
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

        [HttpGet("LastRefreshTime")]
        public ActionResult<LastRefresh> GetLastRefreshTime()
        {
            var lastRefreshTime = context.OddsBoostRepository.Read().Max(l => l.LastRefresh);

            return new LastRefresh { LastRefreshTime = lastRefreshTime };
        }
    }
}
