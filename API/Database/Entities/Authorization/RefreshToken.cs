namespace API.Database.Entities.Authorization;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [NotMapped]
    public string Token { get; set; } = null!;

    public byte[] TokenHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public AppUser User { get; set; } = null!;
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public const int TokenHashLength = 32;
    public static readonly int TokenBase64Length = (int) Math.Ceiling(TokenHashLength / 3.0) * 4;

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Ignore(x => x.Token);

        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => new { x.TokenHash, x.ExpiresAt });

        builder.Property(x => x.TokenHash).HasMaxLength(TokenHashLength);

        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
    }
}