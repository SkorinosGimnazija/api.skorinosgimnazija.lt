namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccomplishmentConfiguration : IEntityTypeConfiguration<Accomplishment>
{
    public void Configure(EntityTypeBuilder<Accomplishment> builder)
    {
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Scale).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AdditionalTeachers).WithOne(x => x.Accomplishment).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Students).WithOne(x => x.Accomplishment).OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name).HasMaxLength(256);

        builder.HasIndex(x => x.Date);
    }
}