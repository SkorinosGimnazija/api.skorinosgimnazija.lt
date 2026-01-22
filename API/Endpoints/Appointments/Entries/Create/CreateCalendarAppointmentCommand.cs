namespace API.Endpoints.Appointments.Entries.Create;

using API.Services.Calendar;
using JetBrains.Annotations;

public sealed record CreateCalendarAppointmentCommand : ICommand
{
    public required Guid AppointmentId { get; init; }
}

[UsedImplicitly]
public sealed class CreateCalendarAppointmentCommandHandler(
    ILogger<CreateCalendarAppointmentCommandHandler> logger,
    IDbContextFactory<AppDbContext> dbContextFactory,
    ICalendarService calendarService)
    : ICommandHandler<CreateCalendarAppointmentCommand>
{
    public async Task ExecuteAsync(CreateCalendarAppointmentCommand command, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        var appointment = await dbContext.Appointments.AsNoTracking()
                              .Where(x => x.Id == command.AppointmentId)
                              .Select(x => new CalendarAppointmentRequest
                              {
                                  Id = x.Id,
                                  Title = x.AppointmentDate.Type.Name,
                                  HostName = x.Host.Name,
                                  AttendeeName = x.AttendeeName,
                                  Note = x.Note,
                                  HostEmail = x.Host.Email,
                                  AttendeeEmail = x.AttendeeEmail,
                                  StartDate = x.AppointmentDate.Date,
                                  DurationInMinutes = x.AppointmentDate.Type.DurationInMinutes,
                                  IsOnline = x.AppointmentDate.Type.IsOnline,
                                  AdditionalInvitees = x.AppointmentDate.Type.AdditionalInvitees
                                      .Select(i => i.Email)
                                      .ToList()
                              })
                              .FirstOrDefaultAsync(ct);

        if (appointment is null)
        {
            logger.LogError("Appointment {AppointmentId} not found", command.AppointmentId);
            return;
        }

        var calendarAppointment = await calendarService.CreateAppointmentAsync(appointment, ct);

        await dbContext.Appointments
            .Where(x => x.Id == command.AppointmentId)
            .ExecuteUpdateAsync(a =>
                a.SetProperty(x => x.Link, calendarAppointment.EventLink), ct);
    }
}