using System;

namespace SportsbookAggregationAPI.SportsbookModels
{
    public class OddsBoostWithSportsbook
    {
        public Guid OddsBoostId { get; set; }
        public string Description { get; set; }
        public int PreviousOdds { get; set; }
        public int BoostedOdds { get; set; }
        public DateTime Date { get; set; }
        public string SiteName { get; set; }
    }
}
