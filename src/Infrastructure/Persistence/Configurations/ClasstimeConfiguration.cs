namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkorinosGimnazija.Domain.Entities.School;

internal class ClasstimeConfiguration : IEntityTypeConfiguration<Classtime>
{
    public void Configure(EntityTypeBuilder<Classtime> builder)
    {
        builder.HasIndex(x => x.Number).IsUnique();
    }
}