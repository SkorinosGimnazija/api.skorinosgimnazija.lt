﻿namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Teacher;

internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    { 
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.EndDate);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(x => x.Title).HasMaxLength(256);
        builder.Property(x => x.Organizer).HasMaxLength(256);
        builder.Property(x => x.CertificateNr).HasMaxLength(100);
    }
}