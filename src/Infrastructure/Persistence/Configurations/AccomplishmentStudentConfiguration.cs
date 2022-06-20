namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccomplishmentStudentConfiguration : IEntityTypeConfiguration<AccomplishmentStudent>
{
    public void Configure(EntityTypeBuilder<AccomplishmentStudent> builder)
    {
        builder.HasOne(x => x.Classroom).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Name).HasMaxLength(128);
    }
}