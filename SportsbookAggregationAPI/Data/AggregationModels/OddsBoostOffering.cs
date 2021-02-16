using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsbookAggregationAPI.Data.AggregationModels
{
    public class OddsBoostOffering
    {
        public string Description { get; set; }
        public int PreviousOdds { get; set; }
        public int BoostedOdds { get; set; }

        public string Sport { get; set; }

        public string Site { get; set; }
        public DateTime Date { get; set; }
    }
}
