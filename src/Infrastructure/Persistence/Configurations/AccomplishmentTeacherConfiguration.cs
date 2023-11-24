namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccomplishmentTeacherConfiguration : IEntityTypeConfiguration<AccomplishmentTeacher>
{
    public void Configure(EntityTypeBuilder<AccomplishmentTeacher> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(128);
    }
}