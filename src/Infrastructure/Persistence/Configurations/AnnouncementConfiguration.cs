namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities.Accomplishments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkorinosGimnazija.Domain.Entities.School;

internal class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(512);

        builder.HasIndex(x => x.StartTime);
    }
}