using System;

namespace SportsbookAggregation.Data.Models
{
    public class GameResult
    {
        public Guid GameResultId { get; set; }        
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public Game Game { get; set; }
        public Guid GameId { get; set; }
    }
}