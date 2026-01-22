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

    private const string IndexUniqueHost = "IX_UniqueHost";
    private const string IndexUniqueAttendeeDate = "IX_UniqueAttendeeDate";
    private const string IndexUniqueHostDate = "IX_UniqueHostDate";

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
            .HasDatabaseName(IndexUniqueHost);

        builder.HasIndex(x => new { x.AttendeeEmail, x.AppointmentDateId })
            .IsUnique()
            .HasDatabaseName(IndexUniqueAttendeeDate);

        builder.HasIndex(x => new { x.HostId, x.AppointmentDateId })
            .IsUnique()
            .HasDatabaseName(IndexUniqueHostDate);

        builder.HasOne(x => x.AppointmentDate)
            .WithMany()
            .HasForeignKey(x => x.AppointmentDateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Host)
            .WithMany()
            .HasForeignKey(x => x.HostId);
    }

    public static string GetErrorMessage(string? constraintName)
    {
        return constraintName switch
        {
            IndexUniqueHost => "Jūs jau esate užsiregistravęs (-usi) pas pasirinktą mokytoją",
            IndexUniqueAttendeeDate => "Pasirinktu laiku jūs jau esate užsiregistravęs (-usi)",
            IndexUniqueHostDate => "Pasirinktas laikas užimtas",
            _ => "Registracijos klaida"
        };
    }
}