namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Accomplishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AccomplishmentAchievementConfiguration : IEntityTypeConfiguration<AccomplishmentAchievement>
{
    public void Configure(EntityTypeBuilder<AccomplishmentAchievement> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(48);
    }
}