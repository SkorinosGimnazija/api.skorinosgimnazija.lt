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

internal class ParentAppointmentConfiguration : IEntityTypeConfiguration<ParentAppointment>
{
    public void Configure(EntityTypeBuilder<ParentAppointment> builder)
    {
        builder.HasOne(x => x.Date).WithMany().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserName).HasMaxLength(100);
        builder.Property(x => x.EventId).HasMaxLength(100);
        builder.Property(x => x.ParentName).HasMaxLength(256);
        builder.Property(x => x.ParentEmail).HasMaxLength(256);

        builder.HasIndex(x => x.UserName);
        builder.HasIndex(x => new { x.ParentEmail, x.DateId }).IsUnique();
        builder.HasIndex(x => new { TeacherId = x.UserId, x.DateId }).IsUnique();
    }
}