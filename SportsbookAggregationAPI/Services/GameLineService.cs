using SportsbookAggregation.SportsBooks;
using SportsbookAggregation.SportsBooks.Mappers;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.AggregationModels;
using SportsbookAggregationAPI.Data.DbModels;
using SportsbookAggregationAPI.Exceptions;
using SportsbookAggregationAPI.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsbookAggregationAPI.Services
{
    public class GameLineService
    {
        private readonly Context dbContext;

        public GameLineService(Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Update(IEnumerable<GameOffering> gameOfferings)
        {
            SetOfferingsToNotAvailable();
            WriteGameOfferings(gameOfferings);
            dbContext.SaveChanges();
        }

        public void WriteGameOfferings(IEnumerable<GameOffering> gameOfferings)
        {
            var teamsNotFound = new List<string>();
            foreach (var gameOffering in gameOfferings)
            {
                try
                {
                    if (gameOffering.DateTime < DateTime.UtcNow)
                        continue;

                    var sportGuid = GetSportId(gameOffering.Sport);
                    var homeTeamId = GetTeamIdFromTeamName(gameOffering.HomeTeam, gameOffering.Sport);
                    var awayTeamId = GetTeamIdFromTeamName(gameOffering.AwayTeam, gameOffering.Sport);
                    var siteId = GetSiteId(gameOffering.Site);

                    var gameId = GetGameId(gameOffering.DateTime, homeTeamId, awayTeamId) ??
                                    CreateGame(gameOffering.DateTime, homeTeamId, awayTeamId, sportGuid);

                    var gameLine = GetGameLine(gameId, siteId);
                    if (gameLine == null)
                        CreateGameLine(gameId, siteId, gameOffering);
                    else
                        UpdateGameLine(gameLine, gameOffering);
                }
                catch (TeamNotFoundException e)
                {
                    teamsNotFound.Add(e.Team);
                }
            }
            if(teamsNotFound.Count > 0)
                APILogger.LogMessage("Needs mapping: " + teamsNotFound.ToString());
        }

        private bool IsCollegeSport(string sport)
        {
            return sport == "NCAAF" || sport == "NCAAB";
        }

        public void SetOfferingsToNotAvailable()
        {
            var allLines = dbContext.GameLineRepository.Read().Where(l => l.IsAvailable);
            foreach (var gameLine in allLines)
                gameLine.IsAvailable = false;
        }

        private void UpdateGameLine(GameLine gameLine, GameOffering gameOffering)
        {
            if (gameLine.OpeningSpread == null)
                gameLine.OpeningSpread = gameOffering.CurrentSpread;
            if (gameLine.OpeningOverUnder == null)
                gameLine.OpeningOverUnder = gameOffering.CurrentOverUnder;
            gameLine.CurrentSpread = gameOffering.CurrentSpread;
            gameLine.CurrentOverUnder = gameOffering.CurrentOverUnder;
            gameLine.HomeMoneyLinePayout = gameOffering.HomeMoneyLinePayout;
            gameLine.AwayMoneyLinePayout = gameOffering.AwayMoneyLinePayout;
            gameLine.OverPayOut = gameOffering.OverPayOut;
            gameLine.UnderPayout = gameOffering.UnderPayout;
            gameLine.HomeSpreadPayout = gameOffering.HomeSpreadPayout;
            gameLine.AwaySpreadPayout = gameOffering.AwaySpreadPayout;
            gameLine.LastRefresh = DateTime.UtcNow;
            gameLine.IsAvailable = true;
        }

        private void CreateGameLine(Guid gameId, Guid siteId, GameOffering gameOffering)
        {
            dbContext.GameLineRepository.CreateWithoutSaving(new GameLine
            {
                GameId = gameId,
                GamblingSiteId = siteId,
                AwayMoneyLinePayout = gameOffering.AwayMoneyLinePayout,
                AwaySpreadPayout = gameOffering.AwaySpreadPayout,
                CurrentSpread = gameOffering.CurrentSpread,
                CurrentOverUnder = gameOffering.CurrentOverUnder,
                HomeMoneyLinePayout = gameOffering.HomeMoneyLinePayout,
                HomeSpreadPayout = gameOffering.HomeSpreadPayout,
                OpeningOverUnder = gameOffering.CurrentOverUnder,
                OpeningSpread = gameOffering.CurrentSpread,
                OverPayOut = gameOffering.OverPayOut,
                UnderPayout = gameOffering.UnderPayout,
                LastRefresh = DateTime.UtcNow,
                IsAvailable = true
            });
        }

        private GameLine GetGameLine(Guid gameId, Guid siteId)
        {
                var games = dbContext.GameLineRepository.Read()
                    .SingleOrDefault(gl => gl.GameId == gameId && gl.GamblingSiteId == siteId);
                return games;
        }

        private Guid CreateGame(DateTime gameOfferingDateTime, Guid homeTeamId, Guid awayTeamId, Guid sportId)
        {
            var game = new Game { AwayTeamId = awayTeamId, HomeTeamId = homeTeamId, TimeStamp = gameOfferingDateTime, SportId = sportId };
            dbContext.GameRepository.Create(game);
            return game.GameId;
        }

        private Guid? GetGameId(DateTime gameTime, Guid homeTeamId, Guid awayTeamId)
        {
            var matchingGames = dbContext.GameRepository.Read()
                .Where(g => g.HomeTeamId == homeTeamId && g.AwayTeamId == awayTeamId && g.TimeStamp.Date == gameTime.Date);
            return matchingGames.FirstOrDefault(g => Math.Abs(g.TimeStamp.Hour - gameTime.Hour) <= 1)?.GameId;
        }

        private Guid GetTeamIdFromTeamName(string teamName, string sport)
        {
            teamName = LocationMapper.GetFullTeamName(teamName, sport);
            if (IsCollegeSport(sport))
                teamName = MascotMapper.GetFullNameUsingCollege(teamName.Trim());

            var team = dbContext.TeamRepository.Read().SingleOrDefault((t => (t.Location + " " + t.Mascot == teamName)));
            if (team == null)
                throw new TeamNotFoundException(teamName);

            return team.TeamId;

        }

        private Guid GetSiteId(string site)
        {
            return dbContext.GamblingSiteRepository.Read().Single(s => s.Name == site).GamblingSiteId;
        }

        private Guid GetSportId(string name)
        {
            if (name == null)
                return dbContext.SportRepository.Read().Single(s => s.Name == "Unknown").SportId;

            var sport = dbContext.SportRepository.Read().FirstOrDefault(s => s.Name == name);
            if (sport == null)
                return dbContext.SportRepository.Read().Single(s => s.Name == "Unknown").SportId;

            return sport.SportId;
        }
    }
}