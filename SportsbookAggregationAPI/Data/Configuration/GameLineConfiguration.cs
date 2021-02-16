using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.DbModels;

namespace SportsbookAggregationAPI.Data.Configuration
{
    public class GameLineConfiguration : IEntityTypeConfiguration<GameLine>
    {
        public void Configure(EntityTypeBuilder<GameLine> builder)
        {
            // default
        }
    }
}