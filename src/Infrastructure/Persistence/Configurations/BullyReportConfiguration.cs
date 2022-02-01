namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Bullies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class BullyReportConfiguration : IEntityTypeConfiguration<BullyReport>
{
    public void Configure(EntityTypeBuilder<BullyReport> builder)
    {
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(x => x.BullyInfo).HasMaxLength(256);
        builder.Property(x => x.VictimInfo).HasMaxLength(256);
        builder.Property(x => x.Location).HasMaxLength(128);
        builder.Property(x => x.Details).HasMaxLength(2048);
    }
}