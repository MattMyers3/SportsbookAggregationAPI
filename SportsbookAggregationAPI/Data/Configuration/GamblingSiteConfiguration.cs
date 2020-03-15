using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.Models;

namespace SportsbookAggregationAPI.Data.Configuration
{
    public class GamblingSiteConfiguration : IEntityTypeConfiguration<GamblingSite>
    {
        public void Configure(EntityTypeBuilder<GamblingSite> builder)
        {
            // default
        }
    }
}