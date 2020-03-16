using System;

namespace SportsbookAggregationAPI.SportsbookModels
{
    public class BestAvailableGameLine
    {
        public double? CurrentOver { get; set; }
        public string OverSite { get; set; }
        
        public double? CurrentUnder { get; set; }
        public string UnderSite { get; set; }
        
        public double? CurrentHomeSpread { get; set; }
        public string HomeSpreadSite { get; set; }
        
        public double? CurrentAwaySpread { get; set; }
        public string AwaySpreadSite { get; set; }
        
        public int? CurrentHomeMoneyLine { get; set; }
        public string HomeMoneyLineSite { get; set; }
        
        public int? CurrentAwayMoneyLine { get; set; }
        public string AwayMoneyLineSite { get; set; }
    }
}