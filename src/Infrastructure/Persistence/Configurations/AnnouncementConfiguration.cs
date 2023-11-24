namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(512);

        builder.HasIndex(x => x.StartTime);
    }
}