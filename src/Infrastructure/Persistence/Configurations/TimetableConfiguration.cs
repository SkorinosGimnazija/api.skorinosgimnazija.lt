namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Timetable;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
{
    public void Configure(EntityTypeBuilder<Timetable> builder)
    {
        builder.Property(x => x.ClassName).HasMaxLength(128);

        builder.HasOne(x => x.Day).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Time).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Room).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.DayId, x.RoomId, x.TimeId }).IsUnique();
    }
}
