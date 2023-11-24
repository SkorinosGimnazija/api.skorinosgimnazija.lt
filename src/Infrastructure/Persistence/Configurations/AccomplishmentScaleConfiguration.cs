namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccomplishmentScaleConfiguration : IEntityTypeConfiguration<AccomplishmentScale>
{
    public void Configure(EntityTypeBuilder<AccomplishmentScale> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(48);
    }
}