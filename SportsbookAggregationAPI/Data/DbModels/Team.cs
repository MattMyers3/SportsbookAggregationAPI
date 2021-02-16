using System;

namespace SportsbookAggregationAPI.Data.DbModels
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public string Mascot { get; set; }
        public string Location { get; set; }
    }
}