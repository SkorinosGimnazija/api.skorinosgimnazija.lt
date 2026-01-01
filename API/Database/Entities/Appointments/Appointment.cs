namespace API.Database.Entities.Appointments;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Appointment
{
    public required Guid Id { get; set; }

    public string? Link { get; set; }

    public int AppointmentDateId { get; set; }

    public int HostId { get; set; }

    public string? Note { get; set; }

    public string AttendeeName { get; set; } = null!;

    public string AttendeeEmail { get; set; } = null!;

    public AppointmentDate AppointmentDate { get; set; } = null!;

    public AppUser Host { get; set; } = null!;
}

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public const int AttendeeNameLength = 256;
    public const int AttendeeEmailLength = 256;
    public const int NoteLength = 256;

    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(x => x.AttendeeName).HasMaxLength(AttendeeNameLength);
        builder.Property(x => x.AttendeeEmail).HasMaxLength(AttendeeEmailLength);
        builder.Property(x => x.Note).HasMaxLength(NoteLength);

        builder.HasIndex(x => x.HostId);
        builder.HasIndex(x => x.AppointmentDateId);
        builder.HasIndex(x => x.AttendeeEmail);

        builder.HasIndex(x => new { x.HostId, x.AttendeeEmail })
            .IsUnique()
            .HasDatabaseName("IX_UniqueHost");

        builder.HasIndex(x => new { x.AttendeeEmail, x.AppointmentDateId })
            .IsUnique()
            .HasDatabaseName("IX_UniqueAttendeeDate");

        builder.HasIndex(x => new { x.HostId, x.AppointmentDateId })
            .IsUnique()
            .HasDatabaseName("IX_UniqueHostDate");

        builder.HasOne(x => x.AppointmentDate)
            .WithMany()
            .HasForeignKey(x => x.AppointmentDateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Host)
            .WithMany()
            .HasForeignKey(x => x.HostId);
    }
}