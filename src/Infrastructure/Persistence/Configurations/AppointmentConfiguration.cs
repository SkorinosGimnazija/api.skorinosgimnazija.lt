﻿namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasOne(x => x.Date).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.UserName).HasMaxLength(100);
        builder.Property(x => x.UserDisplayName).HasMaxLength(100);
        builder.Property(x => x.EventId).HasMaxLength(100);
        builder.Property(x => x.EventMeetingLink).HasMaxLength(100);
        builder.Property(x => x.AttendeeName).HasMaxLength(256);
        builder.Property(x => x.AttendeeEmail).HasMaxLength(256);
        builder.Property(x => x.AttendeeUserName).HasMaxLength(100);

        builder.HasIndex(x => x.UserName);
        builder.HasIndex(x => x.AttendeeUserName);
        builder.HasIndex(x => new { x.UserName, x.AttendeeEmail }).IsUnique();
        builder.HasIndex(x => new { x.DateId, x.AttendeeEmail }).IsUnique();
        builder.HasIndex(x => new { x.DateId, x.UserName }).IsUnique();
    }
}