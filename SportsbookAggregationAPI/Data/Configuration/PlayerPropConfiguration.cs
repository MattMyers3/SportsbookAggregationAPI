using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.DbModels;

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
