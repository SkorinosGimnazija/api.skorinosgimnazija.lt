namespace API.Endpoints.Appointments.Entries.Delete;

using API.Services.Calendar;
using JetBrains.Annotations;

public sealed record CancelCalendarAppointmentCommand : ICommand
{
    public required Guid AppointmentId { get; init; }
}

[UsedImplicitly]
public sealed class CancelCalendarAppointmentCommandHandler(
    ILogger<CancelCalendarAppointmentCommandHandler> logger,
    ICalendarService calendarService)
    : ICommandHandler<CancelCalendarAppointmentCommand>
{
    public async Task ExecuteAsync(CancelCalendarAppointmentCommand command, CancellationToken ct)
    {
        var deleted = await calendarService.CancelAppointmentAsync(command.AppointmentId);
        if (!deleted)
        {
            logger.LogWarning("Appointment '{AppointmentId}' not found", command.AppointmentId);
        }
    }
}