using System.Collections.Generic;

namespace SportsbookAggregationAPI.Data.AggregationModels
{
    public class GameLineUpdateObject
    {
        public IEnumerable<string> Sportsbooks { get; set; }
        public IEnumerable<GameOffering> GameLines { get; set; }
    }

    public class OddsBoostUpdateObject
    {
        public IEnumerable<string> Sportsbooks { get; set; }
        public IEnumerable<OddsBoostOffering> OddsBoosts { get; set; }
    }

    public class PlayerPropUpdateObject
    {
        public IEnumerable<string> Sportsbooks { get; set; }
        public IEnumerable<PlayerPropOffering> PlayerProps { get; set; }
    }
}
