using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SportsbookAggregation.Config
{
    public class ConfigWrapper
    {
        private IConfiguration Configuration;

        public ConfigWrapper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ReadConfig()
        {
            if (Configuration == null)
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{environment}.json");
                Configuration = builder.Build();
            }
        }

        public bool ReadBooleanProperty(string property)
        {
            return Convert.ToBoolean(Configuration[property]);
        }

        public bool ShouldParseBook(string book)
        {
            return Convert.ToBoolean(Configuration.GetSection("SportsBooks")[book]);
        }

        public bool ShouldParseSport(string sport)
        {
            return Convert.ToBoolean(Configuration.GetSection("Sports")[sport]);
        }

        public string GetConnectionString()
        {
            return Configuration.GetConnectionString("SportsbookDatabase");
        }
    }
}
