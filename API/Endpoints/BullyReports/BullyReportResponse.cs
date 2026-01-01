namespace API.Endpoints.BullyReports;

using JetBrains.Annotations;

[PublicAPI]
public record BullyReportResponse
{
    public required int Id { get; init; }

    public required bool IsPublicReport { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateOnly Date { get; init; }

    public required string VictimName { get; init; }

    public required string BullyName { get; init; }

    public required string Location { get; init; }

    public required string Details { get; init; }

    public string? Observers { get; init; }

    public string? Actions { get; init; }

    public string? CreatorName { get; init; }

    public int? CreatorId { get; init; }
}