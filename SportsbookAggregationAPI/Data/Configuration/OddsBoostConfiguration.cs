using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.DbModels;

namespace SportsbookAggregationAPI.Data.Configuration
{
    public class OddsBoostConfiguration : IEntityTypeConfiguration<OddsBoost>
    {
        public void Configure(EntityTypeBuilder<OddsBoost> builder)
        {
            // default
        }
    }
}
