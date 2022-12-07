namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities.CMS;
using SkorinosGimnazija.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.DisplayName).HasMaxLength(100);

        builder.HasIndex(x => x.DisplayName);
    }
}
