using System;

namespace SportsbookAggregation.Data.Models
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public string Mascot { get; set; }
        public string Location { get; set; }

        public Sport Sport { get; set; }
        public Guid SportId { get; set; }
    }
}