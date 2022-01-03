namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Appointments;

internal class AppointmentDateConfiguration : IEntityTypeConfiguration<AppointmentDate>
{
    public void Configure(EntityTypeBuilder<AppointmentDate> builder)
    {
        builder.Property(x => x.Date).HasColumnType("timestamp");
    }
}