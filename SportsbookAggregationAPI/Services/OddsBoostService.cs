using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.AggregationModels;
using SportsbookAggregationAPI.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsbookAggregationAPI.Services
{
    public class OddsBoostService
    {
        private readonly Context dbContext;

        public OddsBoostService(Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Update(IEnumerable<OddsBoostOffering> oddsBoostOffering)
        {
            SetOfferingsToNotAvailable();
            WriteOddsBoosts(oddsBoostOffering);
            dbContext.SaveChanges();
        }

        public void WriteOddsBoosts(IEnumerable<OddsBoostOffering> oddsBoostOfferings)
        {
            foreach (var oddsBoostOffering in oddsBoostOfferings)
            {
                OddsBoost boost = null;
                try
                {
                    boost = dbContext.OddsBoostRepository.Read().SingleOrDefault(o => o.Description == oddsBoostOffering.Description && o.GamblingSite.Name.Equals(oddsBoostOffering.Site, StringComparison.OrdinalIgnoreCase));
                }
                catch (Exception e)
                {
                    var boosts = dbContext.OddsBoostRepository.Read().Where(o => o.Description == oddsBoostOffering.Description && o.GamblingSite.Name.Equals(oddsBoostOffering.Site, StringComparison.OrdinalIgnoreCase)).ToList();
                    foreach (var b in boosts)
                    {
                        dbContext.OddsBoostRepository.Delete(b);
                    }
                }
                if (boost != null)
                    UpdateBoost(boost);
                else
                    CreateBoost(oddsBoostOffering);
            }
        }

        private void UpdateBoost(OddsBoost boost)
        {
            boost.IsAvailable = true;
            boost.LastRefresh = DateTime.UtcNow;
        }

        private void CreateBoost(OddsBoostOffering oddsBoostOffering)
        {
            var siteId = GetSiteId(oddsBoostOffering.Site);
            var sportId = GetSportId(oddsBoostOffering.Sport);
            dbContext.OddsBoostRepository.CreateWithoutSaving(new OddsBoost
            {
                BoostedOdds = oddsBoostOffering.BoostedOdds,
                Date = oddsBoostOffering.Date,
                Description = oddsBoostOffering.Description,
                GamblingSiteId = siteId,
                PreviousOdds = oddsBoostOffering.PreviousOdds,
                SportId = sportId,
                LastRefresh = DateTime.UtcNow,
                IsAvailable = true
            });
        }

        public void SetOfferingsToNotAvailable()
        {
            var allBoosts = dbContext.OddsBoostRepository.Read().Where(b => b.IsAvailable);
            foreach (var boost in allBoosts)
                boost.IsAvailable = false;
        }

        private Guid GetSiteId(string site)
        {
            return dbContext.GamblingSiteRepository.Read().Single(s => s.Name == site).GamblingSiteId;
        }

        private Guid GetSportId(string name)
        {
            if (name == null)
                return dbContext.SportRepository.Read().Single(s => s.Name == "Unknown").SportId;

            var sport = dbContext.SportRepository.Read().FirstOrDefault(s => s.Name == name);
            if (sport == null)
                return dbContext.SportRepository.Read().Single(s => s.Name == "Unknown").SportId;

            return sport.SportId;
        }
    }
}
