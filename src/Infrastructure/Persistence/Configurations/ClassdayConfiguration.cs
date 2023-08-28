namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Timetable;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkorinosGimnazija.Domain.Entities.School;

internal class ClassdayConfiguration : IEntityTypeConfiguration<Classday>
{
    public void Configure(EntityTypeBuilder<Classday> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(48);
        builder.HasIndex(x => x.Number).IsUnique();
    }
}
