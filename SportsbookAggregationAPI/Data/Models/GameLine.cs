using System;
using SportsbookAggregationAPI.Data.Models;

namespace SportsbookAggregation.Data.Models
{
    public class GameLine
    {
        public Guid GameLineId { get; set; }
        public double? OpeningOverUnder { get; set; }
        public double? CurrentOverUnder { get; set; }
        public int? OverPayOut { get; set; }
        public int? UnderPayout { get; set; }
        public double? OpeningSpread { get; set; }
        public double? CurrentSpread { get; set; }
        public int? HomeSpreadPayout { get; set; }
        public int? AwaySpreadPayout { get; set; }
        public int? HomeMoneyLinePayout{ get; set; }
        public int? AwayMoneyLinePayout { get; set; }


        public Game Game { get; set; }
        public Guid GameId { get; set; }

        public GamblingSite GamblingSite { get; set; }
        public Guid GamblingSiteId { get; set; }
    }
}