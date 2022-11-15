namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities.Bullies;
using SkorinosGimnazija.Domain.Entities.TechReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class TechJournalReportConfiguration : IEntityTypeConfiguration<TechJournalReport>
{
    public void Configure(EntityTypeBuilder<TechJournalReport> builder)
    {
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Place).HasMaxLength(64);
        builder.Property(x => x.Details).HasMaxLength(512);
        builder.Property(x => x.Notes).HasMaxLength(512);

        builder.HasIndex(x => x.IsFixed);
    }
}
