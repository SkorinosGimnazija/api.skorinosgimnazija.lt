namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AppointmentReservedDateConfiguration : IEntityTypeConfiguration<AppointmentReservedDate>
{
    public void Configure(EntityTypeBuilder<AppointmentReservedDate> builder)
    {
        builder.HasOne(x => x.Date).WithMany().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserName).HasMaxLength(100);

        builder.HasIndex(x => x.UserName);
    }
}