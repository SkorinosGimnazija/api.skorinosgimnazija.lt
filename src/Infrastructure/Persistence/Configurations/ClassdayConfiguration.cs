namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class ClassdayConfiguration : IEntityTypeConfiguration<Classday>
{
    public void Configure(EntityTypeBuilder<Classday> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(48);
        builder.HasIndex(x => x.Number).IsUnique();
    }
}