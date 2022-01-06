namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkorinosGimnazija.Domain.Entities.Appointments;

internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasOne(x => x.Date).WithMany().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Type).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.UserName).HasMaxLength(100);
        builder.Property(x => x.EventId).HasMaxLength(100);
        builder.Property(x => x.AttendeeName).HasMaxLength(256);
        builder.Property(x => x.AttendeeEmail).HasMaxLength(256);

        builder.HasIndex(x => x.UserName);
        builder.HasIndex(x => x.AttendeeEmail);
        builder.HasIndex(x => new { x.DateId, x.AttendeeEmail }).IsUnique();
        builder.HasIndex(x => new { x.DateId, x.UserName }).IsUnique();
    }
}