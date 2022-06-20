namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccomplishmentClassroomConfiguration : IEntityTypeConfiguration<AccomplishmentClassroom>
{
    public void Configure(EntityTypeBuilder<AccomplishmentClassroom> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(48);
    }
}