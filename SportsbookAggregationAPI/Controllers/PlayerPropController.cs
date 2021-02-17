using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.AggregationModels;
using SportsbookAggregationAPI.Data.DbModels;
using SportsbookAggregationAPI.Services;
using SportsbookAggregationAPI.SportsbookModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace SportsbookAggregationAPI.Controllers
{
    [ApiController]
    public class PlayerPropController : ControllerBase
    {
        private readonly Context context;
        public const string Over = "Over";
        public const string Under = "Under";

        public PlayerPropController(Context context)
        {
            this.context = context;
        }

        [Route("api/Games/{id}/BestProps")]
        [HttpGet]
        public ActionResult<List<BestAvailablePlayerProp>> GetBestAvailablePlayerProps(Guid id, [FromQuery] string sportsbooks)
        {
            if (sportsbooks == null)
                return NotFound();

            var sportsbooksArray = sportsbooks?.Split(',');
            var availablePlayerProps = context.PlayerPropRepository.Read().Where(p => p.GameId == id && p.IsAvailable).ToList();
            if (!availablePlayerProps.Any())
                return NotFound();

            var gamblingSites = context.GamblingSiteRepository.Read().ToList();

            var playerProps = new List<BestAvailablePlayerProp>();

            var playerPropGroups = availablePlayerProps.GroupBy(prop => prop, new PlayerNameComparer());

            foreach(var playerPropGroup in playerPropGroups)
            {
                var bestAvailablePlayerProp = new BestAvailablePlayerProp();
                foreach(var prop in playerPropGroup)
                {
                    var gamblingSiteName = gamblingSites.First(s => s.GamblingSiteId == prop.GamblingSiteId).Name;
                    if (sportsbooksArray != null && !sportsbooksArray.Contains(gamblingSiteName))
                        continue;
                    if (PropIsBetterOverBet(bestAvailablePlayerProp, prop) || PropIsBetterUnderBet(bestAvailablePlayerProp, prop) || PropIsBetterOtherBet(bestAvailablePlayerProp, prop))
                    {
                        bestAvailablePlayerProp.CurrentPayout = prop.Payout;
                        bestAvailablePlayerProp.PropValue = prop.PropValue;
                        bestAvailablePlayerProp.CurrentSite = gamblingSiteName;
                        bestAvailablePlayerProp.PlayerName = prop.PlayerName;
                        bestAvailablePlayerProp.PropDescription = prop.Description;
                        bestAvailablePlayerProp.PropTypeDescription = prop.PropBetType;
                    }
                }
                if (bestAvailablePlayerProp.PlayerName != null)
                    playerProps.Add(bestAvailablePlayerProp);
            }
            return playerProps;
        }

        [Route("api/[controller]")]
        [Authorize]
        [HttpPut]
        public HttpStatusCode Update(PlayerPropUpdateObject playerPropUpdateObject)
        {
            if (HttpContext.User.Claims.Single(c => c.Type == "cid")?.Value != "0oa60prueRe8fdEkB5d6") //Aggregator account
                return HttpStatusCode.Unauthorized;

            using (var dbContext = new Context())
            {
                var gameLineService = new PlayerPropService(dbContext);
                using (var dbContextTransaction = dbContext.Database.BeginTransaction())
                {
                    gameLineService.Update(playerPropUpdateObject);
                    dbContextTransaction.Commit();
                }
            }
            return HttpStatusCode.NoContent;
        }

        private static bool PropIsBetterOtherBet(BestAvailablePlayerProp bestAvailablePlayerProp, PlayerProp prop)
        {
            return prop.Description != Under && prop.Description != Over && (bestAvailablePlayerProp.CurrentPayout == null || prop.Payout > bestAvailablePlayerProp.CurrentPayout);
        }

        private static bool PropIsBetterUnderBet(BestAvailablePlayerProp bestAvailablePlayerProp, PlayerProp prop)
        {
            return prop.Description == Under &&
                                    (bestAvailablePlayerProp.CurrentPayout == null || prop.PropValue > bestAvailablePlayerProp.PropValue ||
                                    (prop.PropValue == bestAvailablePlayerProp.PropValue && prop.Payout > bestAvailablePlayerProp.CurrentPayout));
        }

        private static bool PropIsBetterOverBet(BestAvailablePlayerProp bestAvailablePlayerProp, PlayerProp prop)
        {
            return prop.Description == Over &&
                                    (bestAvailablePlayerProp.CurrentPayout == null || prop.PropValue < bestAvailablePlayerProp.PropValue ||
                                    (prop.PropValue == bestAvailablePlayerProp.PropValue && prop.Payout > bestAvailablePlayerProp.CurrentPayout));
        }
    }

    public class PlayerNameComparer : IEqualityComparer<PlayerProp>
    {
        public bool Equals([AllowNull] PlayerProp x, [AllowNull] PlayerProp y)
        {
            return (FuzzySharp.Fuzz.Ratio(x.PlayerName, y.PlayerName) > 76) && x.PropBetType == y.PropBetType && x.Description == y.Description;
        }

        public int GetHashCode([DisallowNull] PlayerProp prop)
        {
            var hash = new HashCode();
            hash.Add(prop.PropBetType);
            hash.Add(prop.GameId);
            hash.Add(prop.Description);
            return hash.ToHashCode();
        }
    }
}
