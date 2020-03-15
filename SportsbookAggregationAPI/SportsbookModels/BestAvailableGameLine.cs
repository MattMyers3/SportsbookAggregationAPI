using System;

namespace SportsbookAggregationAPI.SportsbookModels
{
    public class BestAvailableGameLine
    {
        public double? CurrentOver { get; set; }
        public Guid OverSite { get; set; }
        
        public double? CurrentUnder { get; set; }
        public Guid UnderSite { get; set; }
        
        public double? CurrentHomeSpread { get; set; }
        public Guid HomeSpreadSite { get; set; }
        
        public double? CurrentAwaySpread { get; set; }
        public Guid AwaySpreadSite { get; set; }
        
        public int? CurrentHomeMoneyLine { get; set; }
        public Guid HomeMoneyLineSite { get; set; }
        
        public int? CurrentAwayMoneyLine { get; set; }
        public Guid AwayMoneyLineSite { get; set; }
    }
}