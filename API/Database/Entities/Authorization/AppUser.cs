namespace API.Database.Entities.Authorization;

using API.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NormalizedName { get; private init; } = null!;

    public string Email { get; set; } = null!;

    public string? JobTitle { get; set; }

    public string? Location { get; set; }

    public bool IsSuspended { get; set; }

    public bool IsTeacher { get; set; }

    public List<string> Roles { get; set; } = [];
}

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.UserName).IsUnique();
        builder.HasIndex(x => x.NormalizedName).GinTrigram();
        builder.HasIndex(x => new { x.IsTeacher, x.IsSuspended, x.Name });
        builder.GenerateNormalized(s => s.Name, t => t.NormalizedName);

        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.UserName).HasMaxLength(128);
    }
}