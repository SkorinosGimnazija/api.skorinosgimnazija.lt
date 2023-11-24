namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.TechReports;
using Microsoft.EntityFrameworkCore;
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