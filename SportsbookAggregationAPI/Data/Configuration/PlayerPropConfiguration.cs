using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.Models;

namespace SportsbookAggregationAPI.Data.Configuration
{
    public class PlayerPropConfiguration : IEntityTypeConfiguration<PlayerProp>
    {
        public void Configure(EntityTypeBuilder<PlayerProp> builder)
        {
            //default
        }
    }
}
