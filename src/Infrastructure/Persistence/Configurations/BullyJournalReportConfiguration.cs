namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Bullies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class BullyJournalReportConfiguration : IEntityTypeConfiguration<BullyJournalReport>
{
    public void Configure(EntityTypeBuilder<BullyJournalReport> builder)
    {
        builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.BullyInfo).HasMaxLength(256);
        builder.Property(x => x.VictimInfo).HasMaxLength(256);
        builder.Property(x => x.Details).HasMaxLength(2048);
        builder.Property(x => x.Actions).HasMaxLength(2048);

        builder.HasIndex(x => x.Date);
    }
}