namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class ClasstimeConfiguration : IEntityTypeConfiguration<Classtime>
{
    public void Configure(EntityTypeBuilder<Classtime> builder)
    {
        builder.HasIndex(x => x.Number).IsUnique();
    }
}