namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.DisplayName).HasMaxLength(100);

        builder.HasIndex(x => x.DisplayName);
    }
}