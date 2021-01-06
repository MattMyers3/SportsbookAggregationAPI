using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsbookAggregationAPI.Data.Models;

namespace SportsbookAggregationAPI.Data.Configuration
{
    public class PropBetTypeConfiguration : IEntityTypeConfiguration<PropBetType>
    {
        public void Configure(EntityTypeBuilder<PropBetType> builder)
        {
            //default
        }
    }
}
