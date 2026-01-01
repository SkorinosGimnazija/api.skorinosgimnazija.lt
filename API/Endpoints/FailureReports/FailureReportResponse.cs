namespace API.Endpoints.FailureReports;

using JetBrains.Annotations;

[PublicAPI]
public record FailureReportResponse
{
    public required int Id { get; init; }

    public required string CreatorName { get; init; }

    public required int CreatorId { get; init; }

    public required string Details { get; init; }

    public required string Location { get; init; }

    public required bool? IsFixed { get; init; }

    public required DateTime ReportDate { get; init; }

    public required DateTime? FixDate { get; init; }
}