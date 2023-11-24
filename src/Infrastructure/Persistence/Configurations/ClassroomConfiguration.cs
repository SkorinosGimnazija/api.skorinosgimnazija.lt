namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.ToTable("AccomplishmentClassrooms");
        builder.Property(x => x.Name).HasMaxLength(48);
        builder.HasIndex(x => x.Number).IsUnique();
    }
}