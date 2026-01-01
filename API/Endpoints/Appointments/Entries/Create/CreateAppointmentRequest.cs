namespace API.Endpoints.Appointments.Entries.Create;

using API.Endpoints.Appointments.Entries.Public;
using API.Extensions;
using JetBrains.Annotations;

[PublicAPI]
public record CreateAppointmentRequest
{
    public required int TypeId { get; init; }

    public required int HostId { get; init; }

    public required int DateId { get; init; }
}

public class CreateAppointmentRequestValidator : Validator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x).MustAsync(ValidData).WithMessage("Registracijos klaida");
    }

    private async Task<bool> ValidData(CreateAppointmentRequest req, CancellationToken ct)
    {
        var dbContext = Resolve<AppDbContext>();

        if (!await IsValidHost(dbContext, req, ct))
        {
            return false;
        }

        if (!await IsValidType(dbContext, req, ct))
        {
            return false;
        }

        if (!await IsValidDate(dbContext, req, ct))
        {
            return false;
        }

        return true;
    }

    private static async Task<bool> IsValidHost(
        AppDbContext dbContext, CreateAppointmentRequest req, CancellationToken ct)
    {
        var hostValid = await dbContext.Users.Teachers()
                            .AnyAsync(x => x.Id == req.HostId, ct);

        return hostValid;
    }

    private static async Task<bool> IsValidType(
        AppDbContext dbContext, CreateAppointmentRequest req, CancellationToken ct)
    {
        var isPublic = req is CreateAppointmentPublicRequest;
        var typeValid = await dbContext.AppointmentTypes.AsNoTracking()
                            .Where(x => x.Id == req.TypeId &&
                                        x.IsPublic == isPublic &&
                                        x.RegistrationEndsAt > DateTime.UtcNow.AddHours(
                                            -AppointmentSettings.RegistrationSkewInHours))
                            .AnyAsync(x =>
                                    x.ExclusiveHosts.Count == 0 ||
                                    x.ExclusiveHosts.Any(h => h.Id == req.HostId),
                                ct);

        return typeValid;
    }

    private static async Task<bool> IsValidDate(
        AppDbContext dbContext, CreateAppointmentRequest req, CancellationToken ct)
    {
        var dateValid = await dbContext.AppointmentDates.AsNoTracking()
                            .AnyAsync(x =>
                                x.Id == req.DateId &&
                                x.TypeId == req.TypeId &&
                                x.Date > DateTime.UtcNow.AddHours(
                                    AppointmentSettings.AvailableDateOffsetInHours -
                                    AppointmentSettings.RegistrationSkewInHours) &&
                                !dbContext.AppointmentReservedDates.Any(r =>
                                    r.DateId == x.Id &&
                                    r.HostId == req.HostId), ct);

        return dateValid;
    }
}