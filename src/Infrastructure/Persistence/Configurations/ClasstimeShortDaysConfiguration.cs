namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class ClasstimeShortDaysConfiguration : IEntityTypeConfiguration<ClasstimeShortDay>
{
    public void Configure(EntityTypeBuilder<ClasstimeShortDay> builder)
    {
        builder.HasIndex(x => x.Date).IsUnique();
    }
}