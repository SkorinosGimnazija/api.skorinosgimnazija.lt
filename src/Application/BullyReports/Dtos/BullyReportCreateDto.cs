namespace SkorinosGimnazija.Application.BullyReports.Dtos;

public record BullyReportCreateDto
{
    public string BullyInfo { get; init; } = default!;

    public string VictimInfo { get; init; } = default!;

    public string? Details { get; init; }

    public string Location { get; init; } = default!;

    public string CaptchaToken { get; init; } = default!;

    public DateTime Date { get; init; }
}