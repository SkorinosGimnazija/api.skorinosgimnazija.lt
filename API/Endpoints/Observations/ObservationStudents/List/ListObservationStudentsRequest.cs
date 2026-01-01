namespace API.Endpoints.Observations.ObservationStudents.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListObservationStudentsRequest
{
    [QueryParam]
    public bool? ShowEnabledOnly { get; init; }
}