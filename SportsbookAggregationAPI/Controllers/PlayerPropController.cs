using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.Models;
using SportsbookAggregationAPI.SportsbookModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/Games/{id}/BestProps")]
    [ApiController]
    public class PlayerPropController : ControllerBase
    {
        private readonly Context context;

        public PlayerPropController(Context context)
        {
            this.context = context;
        }

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
            var propBetTypes = context.PropBetTypeRepository.Read().ToList();

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
                    if (bestAvailablePlayerProp.CurrentPayout == null || prop.Payout > bestAvailablePlayerProp.CurrentPayout)
                    {
                        bestAvailablePlayerProp.CurrentPayout = prop.Payout;
                        bestAvailablePlayerProp.PropValue = prop.PropValue;
                        bestAvailablePlayerProp.CurrentSite = gamblingSiteName;
                        bestAvailablePlayerProp.PlayerName = prop.PlayerName;
                        bestAvailablePlayerProp.PropDescription = prop.Description;
                        bestAvailablePlayerProp.PropTypeDescription = propBetTypes.First(t => t.PropBetTypeId == prop.PropBetTypeId).Description;
                    }
                }
                if(bestAvailablePlayerProp.PlayerName != null)
                    playerProps.Add(bestAvailablePlayerProp);
            }
            return playerProps;
        }
    }

    public class PlayerNameComparer : IEqualityComparer<PlayerProp>
    {
        public bool Equals([AllowNull] PlayerProp x, [AllowNull] PlayerProp y)
        {
            return (FuzzySharp.Fuzz.Ratio(x.PlayerName, y.PlayerName) > 70) && x.PropBetTypeId == y.PropBetTypeId && x.Description == y.Description;
        }

        public int GetHashCode([DisallowNull] PlayerProp prop)
        {
            var hash = new HashCode();
            hash.Add(prop.PropBetTypeId);
            hash.Add(prop.GameId);
            hash.Add(prop.Description);
            return hash.ToHashCode();
        }
    }
}
