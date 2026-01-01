namespace API.Endpoints.School.ShortDays.Update;

using API.Endpoints.School.ShortDays.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateShortDayRequest : CreateShortDayRequest
{
    public required int Id { get; init; }
}