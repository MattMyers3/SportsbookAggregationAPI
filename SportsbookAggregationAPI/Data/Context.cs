﻿#nullable enable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SportsbookAggregationAPI.Data.Configuration;
using SportsbookAggregationAPI.Data.DbModels;
using System.Data.Common;

namespace SportsbookAggregationAPI.Data
{
    public class Context : DbContext
    {
        private readonly DbConnection connection;

        public Context() : this(GetDbConnection())
        {
        }

        public Context(DbConnection connection) => this.connection = connection;
        
        public IRepository<GamblingSite> GamblingSiteRepository => new SqlServerRepository<GamblingSite>(this);
        public IRepository<Game> GameRepository => new SqlServerRepository<Game>(this);
        public IRepository<GameLine> GameLineRepository => new SqlServerRepository<GameLine>(this);
        public IRepository<Team> TeamRepository => new SqlServerRepository<Team>(this);
        public IRepository<Sport> SportRepository => new SqlServerRepository<Sport>(this);
        public IRepository<GameResult> GameResultRepository => new SqlServerRepository<GameResult>(this);
        public IRepository<OddsBoost> OddsBoostRepository => new SqlServerRepository<OddsBoost>(this);
        public IRepository<UserSettings> UserSettingsRepository => new SqlServerRepository<UserSettings>(this);
        public IRepository<PlayerProp> PlayerPropRepository => new SqlServerRepository<PlayerProp>(this);


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GamblingSiteConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new GameLineConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new SportConfiguration());
            modelBuilder.ApplyConfiguration(new GameResultConfiguration());
            modelBuilder.ApplyConfiguration(new OddsBoostConfiguration());
            modelBuilder.ApplyConfiguration(new UserSettingsConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerPropConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connection);
        }

        private static DbConnection GetDbConnection()
        {
            return new MySqlConnection(Startup.Configuration.GetConnectionString());
        }
    }
}