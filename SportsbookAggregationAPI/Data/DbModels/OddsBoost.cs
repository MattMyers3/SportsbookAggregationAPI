using System;

namespace SportsbookAggregationAPI.Data.DbModels
{
    public class OddsBoost
    {
        public Guid OddsBoostId { get; set; }
        public string Description { get; set; }
        public int PreviousOdds { get; set; }
        public int BoostedOdds { get; set; }

        public Sport Sport { get; set; }
        public Guid SportId { get; set; }

        public GamblingSite GamblingSite { get; set; }
        public Guid GamblingSiteId { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime Date { get; set; }

        public DateTime LastRefresh { get; set; }
    }
}
