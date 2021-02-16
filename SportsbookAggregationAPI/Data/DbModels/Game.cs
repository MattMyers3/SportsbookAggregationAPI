using System;

namespace SportsbookAggregationAPI.Data.DbModels
{
    public class Game
    {
        public Guid GameId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public Sport Sport { get; set; }
        public Guid SportId { get; set; }
    }
}