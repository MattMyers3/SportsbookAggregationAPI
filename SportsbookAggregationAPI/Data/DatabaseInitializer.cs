using System;
using System.Linq;
using SportsbookAggregation.Data.Models;
using SportsbookAggregationAPI.Data.Models;

namespace SportsbookAggregationAPI.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(Context dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.TeamRepository.Read().Any())
            {
                return; // DB has been seeded
            }

            var nbaGuid = Guid.NewGuid();
            var sports = new[]
            {
                new Sport {SportId = nbaGuid, Name = "NBA"}
            };
            foreach (var sport in sports)
            {
                dbContext.SportRepository.Create(sport);
            }
            dbContext.SaveChanges();

            var teams = new[]
            {
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Atlanta", Mascot = "Hawks"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Boston", Mascot = "Celtics"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Brooklyn", Mascot = "Nets"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Charlotte", Mascot = "Hornets"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Chicago", Mascot = "Bulls"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Cleveland", Mascot = "Cavaliers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Dallas", Mascot = "Mavericks"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Denver", Mascot = "Nuggets"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Detroit", Mascot = "Pistons"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Golden State", Mascot = "Warriors"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Houston", Mascot = "Rockets"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Indiana", Mascot = "Pacers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Los Angeles", Mascot = "Clippers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Los Angeles", Mascot = "Lakers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Memphis", Mascot = "Grizzlies"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Miami", Mascot = "Heat"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Milwaukee", Mascot = "Bucks"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Minnesota", Mascot = "Timberwolves"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "New Orleans", Mascot = "Pelicans"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "New York", Mascot = "Knicks"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Oklahoma City", Mascot = "Thunder"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Orlando", Mascot = "Magic"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Philadelphia", Mascot = "76ers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Phoenix", Mascot = "Suns"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Portland", Mascot = "Trail Blazers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Sacramento", Mascot = "Kings"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "San Antonio", Mascot = "Spurs"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Toronto", Mascot = "Raptors"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Utah", Mascot = "Jazz"},
                new Team {TeamId = Guid.NewGuid(), SportId = nbaGuid, Location = "Washington", Mascot = "Wizards"},
            };
            foreach (var team in teams)
            {
                dbContext.TeamRepository.Create(team);
            }
            dbContext.GamblingSiteRepository.Create(new GamblingSite {GamblingSiteId = Guid.NewGuid(), Name = "Fanduel"});
            dbContext.GamblingSiteRepository.Create(new GamblingSite {GamblingSiteId = Guid.NewGuid(), Name = "FoxBet"});
            dbContext.SaveChanges();
        }
    }
}