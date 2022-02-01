namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Teacher;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.EndDate);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(x => x.Title).HasMaxLength(512);
        builder.Property(x => x.Organizer).HasMaxLength(256);
        builder.Property(x => x.CertificateNr).HasMaxLength(100);
    }
}