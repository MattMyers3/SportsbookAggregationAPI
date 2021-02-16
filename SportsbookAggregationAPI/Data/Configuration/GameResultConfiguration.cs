using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.DbModels;

namespace SportsbookAggregationAPI.Data.Configuration
{
    public class GameResultConfiguration : IEntityTypeConfiguration<GameResult>
    {
        public void Configure(EntityTypeBuilder<GameResult> builder)
        {
            // default
        }
    }
}