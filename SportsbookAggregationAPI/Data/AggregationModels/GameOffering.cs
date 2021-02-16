using System;

namespace SportsbookAggregationAPI.Data.AggregationModels
{
    public class GameOffering
    {
        public double? CurrentOverUnder { get; set; }
        public int? OverPayOut { get; set; }
        public int? UnderPayout { get; set; }
        public double? CurrentSpread { get; set; }
        public int? HomeSpreadPayout { get; set; }
        public int? AwaySpreadPayout { get; set; }
        public int? HomeMoneyLinePayout { get; set; }
        public int? AwayMoneyLinePayout { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Sport { get; set; }
        public string Site { get; set; }
        public DateTime DateTime { get; set; }
    }
}