namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Bullies;

internal class BullyReportConfiguration : IEntityTypeConfiguration<BullyReport>
{
    public void Configure(EntityTypeBuilder<BullyReport> builder)
    {
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(x => x.BullyInfo).HasMaxLength(256);
        builder.Property(x => x.VictimInfo).HasMaxLength(256);
        builder.Property(x => x.ReporterInfo).HasMaxLength(256);
        builder.Property(x => x.Location).HasMaxLength(128);
        builder.Property(x => x.Details).HasMaxLength(2048);
    }
}