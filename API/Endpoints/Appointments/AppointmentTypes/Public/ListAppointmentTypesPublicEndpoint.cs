namespace API.Endpoints.Appointments.AppointmentTypes.Public;

public sealed class ListAppointmentTypesPublicEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<AppointmentTypeResponse>, AppointmentTypeMapper>
{
    public override void Configure()
    {
        Get("public/appointments/types");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.AppointmentTypes.AsNoTracking()
                           .Where(x => x.IsPublic && x.RegistrationEndsAt > DateTime.UtcNow)
                           .OrderByDescending(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}