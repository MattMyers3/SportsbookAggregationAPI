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

            var nbaGuid = Guid.NewGuid();
            var nflGuid = Guid.NewGuid();

            var sports = new[]
            {
                new Sport {SportId = nbaGuid, Name = "NBA"},
                new Sport {SportId = nflGuid, Name = "NFL"}
            };
            foreach (var sport in sports)
            {
                if (!dbContext.SportRepository.Read().Where(s => s.Name == sport.Name).Any())
                {
                    dbContext.SportRepository.Create(sport);
                }
            }
            dbContext.SaveChanges();

            var teams = new[]
            {
                //NBA
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

                //NFL
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Arizona", Mascot = "Cardinals"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Atlanta", Mascot = "Falcons"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Baltimore", Mascot = "Ravens"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Buffalo", Mascot = "Bills"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Carolina", Mascot = "Panthers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Chicago", Mascot = "Bears"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Cincinnati", Mascot = "Bengals"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Cleveland", Mascot = "Browns"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Dallas", Mascot = "Cowboys"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Denver", Mascot = "Broncos"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Detroit", Mascot = "Lions"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Green Bay", Mascot = "Packers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Houston", Mascot = "Texans"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Indianapolis", Mascot = "Colts"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Jacksonville", Mascot = "Jaguars"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Kansas City", Mascot = "Chiefs"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Los Angeles", Mascot = "Chargers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Los Angeles", Mascot = "Rams"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Las Vegas", Mascot = "Raiders"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Miami", Mascot = "Dolphins"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Minnesota", Mascot = "Vikings"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "New England", Mascot = "Patriots"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "New Orleans", Mascot = "Saints"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "New York", Mascot = "Giants"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "New York", Mascot = "Jets"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Philadelphia", Mascot = "Eagles"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Pittsburgh", Mascot = "Steelers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Seattle", Mascot = "Seahawks"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "San Francisco", Mascot = "49ers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Tampa Bay", Mascot = "Buccaneers"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Tennessee", Mascot = "Titans"},
                new Team {TeamId = Guid.NewGuid(), SportId = nflGuid, Location = "Washington", Mascot = "Football Team"},
            };

            foreach (var team in teams)
            {
                if (!dbContext.TeamRepository.Read().Where(t => t.Location == team.Location && t.Mascot == team.Mascot).Any())
                {
                    dbContext.TeamRepository.Create(team);
                }
            }
            string[] books = { "Fanduel", "FoxBet", "DraftKings", "BetRivers" };
            foreach (var book in books)
            {
                if (!dbContext.GamblingSiteRepository.Read().Where(s => s.Name == book).Any())
                {
                    dbContext.GamblingSiteRepository.Create(new GamblingSite { GamblingSiteId = Guid.NewGuid(), Name = book });
                }
            }
            dbContext.SaveChanges();
        }
    }
}