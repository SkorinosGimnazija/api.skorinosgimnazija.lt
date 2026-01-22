namespace API.Database.Entities.Appointments;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppointmentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int DurationInMinutes { get; set; }

    public bool IsPublic { get; set; }

    public bool IsOnline { get; set; }

    public DateTime RegistrationEndsAt { get; set; }

    public List<AppUser> AdditionalInvitees { get; set; } = [];

    public List<AppUser> ExclusiveHosts { get; set; } = [];
}

public class AppointmentTypeConfiguration : IEntityTypeConfiguration<AppointmentType>
{
    public const int NameLength = 128;

    public void Configure(EntityTypeBuilder<AppointmentType> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(NameLength);
        builder.Property(x => x.Description).HasMaxLength(NameLength);

        builder.HasMany(x => x.AdditionalInvitees).WithMany();
        builder.HasMany(x => x.ExclusiveHosts).WithMany();
    }
}