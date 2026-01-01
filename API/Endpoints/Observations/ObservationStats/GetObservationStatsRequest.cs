namespace API.Endpoints.Observations.ObservationStats;

using JetBrains.Annotations;

[PublicAPI]
public record GetObservationStatsRequest
{
    public required int StudentId { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }
}