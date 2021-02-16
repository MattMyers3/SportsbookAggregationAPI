using SportsbookAggregation.SportsBooks;
using SportsbookAggregation.SportsBooks.Mappers;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.AggregationModels;
using SportsbookAggregationAPI.Data.DbModels;
using SportsbookAggregationAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsbookAggregationAPI.Services
{
    public class PlayerPropService
    {
        private readonly Context dbContext;

        public PlayerPropService(Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Update(IEnumerable<PlayerPropOffering> playerPropOfferings)
        {
            SetOfferingsToNotAvailable();
            WritePlayerProps(playerPropOfferings);
            dbContext.SaveChanges();
        }
        public void WritePlayerProps(IEnumerable<PlayerPropOffering> playerProps)
        {
            foreach (var playerProp in playerProps)
            {
                var playerPropInDatabase = TryGetPlayerProp(playerProp);
                if (playerPropInDatabase == null)
                    CreatePlayerProp(playerProp);
                else
                    UpdatePlayerProp(playerPropInDatabase, playerProp);
            }
        }

        private void CreatePlayerProp(PlayerPropOffering playerProp)
        {
            var sportGuid = GetSportId(playerProp.Sport);
            var homeTeamId = GetTeamIdFromTeamName(playerProp.HomeTeam, playerProp.Sport);
            var awayTeamId = GetTeamIdFromTeamName(playerProp.AwayTeam, playerProp.Sport);
            var gameId = GetGameId(playerProp.DateTime, homeTeamId, awayTeamId);

            if (gameId == null) // if we dont have lines for the game yet don't create it for players props. We won't display it anyway
                return;

            var gamblingSiteId = GetSiteId(playerProp.Site);

            dbContext.PlayerPropRepository.CreateWithoutSaving(new PlayerProp
            {
                IsAvailable = true,
                LastRefresh = DateTime.UtcNow,
                Payout = playerProp.Payout,
                PlayerName = playerProp.PlayerName,
                Description = playerProp.OutcomeDescription,
                PropValue = playerProp.PropValue,
                PropBetType = playerProp.Description,
                GamblingSiteId = gamblingSiteId,
                GameId = gameId.Value
            });
        }

        private void UpdatePlayerProp(PlayerProp playerPropInDatabase, PlayerPropOffering playerPropOffering)
        {
            playerPropInDatabase.IsAvailable = true;
            playerPropInDatabase.LastRefresh = DateTime.UtcNow;
            playerPropInDatabase.Payout = playerPropOffering.Payout;
            playerPropInDatabase.PropValue = playerPropOffering.PropValue;
        }

        private PlayerProp TryGetPlayerProp(PlayerPropOffering playerProp)
        {
            var homeTeamId = GetTeamIdFromTeamName(playerProp.HomeTeam, playerProp.Sport);
            var awayTeamId = GetTeamIdFromTeamName(playerProp.AwayTeam, playerProp.Sport);
            var gameId = GetGameId(playerProp.DateTime, homeTeamId, awayTeamId);
            if (gameId == null)
                return null;

            var gamblingSiteId = GetSiteId(playerProp.Site);

            var test = dbContext.PlayerPropRepository.Read().Where(p => p.GameId == gameId);

            return dbContext.PlayerPropRepository.Read().FirstOrDefault(p => p.GameId == gameId
                    && p.PropBetType == playerProp.Description && p.Description == playerProp.OutcomeDescription
                    && p.PlayerName == playerProp.PlayerName && p.GamblingSiteId == gamblingSiteId);
        }
        public void SetOfferingsToNotAvailable()
        {
            var allPlayerProps = dbContext.PlayerPropRepository.Read().Where(p => p.IsAvailable);
            foreach (var prop in allPlayerProps)
                prop.IsAvailable = false;
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
                throw new TeamNotFoundException("Need to add a mapping for the following team: " + teamName);

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

        private bool IsCollegeSport(string sport)
        {
            return sport == "NCAAF" || sport == "NCAAB";
        }
    }
}
