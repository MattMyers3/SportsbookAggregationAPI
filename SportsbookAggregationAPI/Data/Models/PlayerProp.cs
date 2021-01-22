using SportsbookAggregation.Data.Models;
using System;

namespace SportsbookAggregationAPI.Data.Models
{
    public class PlayerProp
    {
        public Guid PlayerPropId { get; set; }
        public string PropBetType { get; set; }

        public Game Game { get; set; }
        public Guid GameId { get; set; }

        public GamblingSite GamblingSite { get; set; }
        public Guid GamblingSiteId { get; set; }

        public string PlayerName { get; set; }

        public string Description { get; set; }

        public double? PropValue { get; set; }

        public int? Payout { get; set; }

        public DateTime LastRefresh { get; set; }

        public bool IsAvailable { get; set; }
    }
}
