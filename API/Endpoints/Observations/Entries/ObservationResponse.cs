namespace API.Endpoints.Observations.Entries;

using JetBrains.Annotations;

[PublicAPI]
public record ObservationResponse
{
    public required int Id { get; init; }

    public required DateOnly Date { get; init; }

    public string? Note { get; init; }

    public required string CreatorName { get; init; }

    public required string StudentName { get; init; }

    public required int StudentId { get; init; }

    public required string LessonName { get; init; }

    public required int LessonId { get; init; }

    public required List<int> OptionIds { get; init; }
}