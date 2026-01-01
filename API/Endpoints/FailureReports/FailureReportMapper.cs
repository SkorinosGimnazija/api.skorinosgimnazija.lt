namespace API.Endpoints.FailureReports;

using API.Database.Entities.FailureReports;
using API.Endpoints.FailureReports.Create;

public sealed class FailureReportMapper
    : Mapper<CreateFailureReportRequest, FailureReportResponse, FailureReport>
{
    public override FailureReport ToEntity(CreateFailureReportRequest r)
    {
        return new()
        {
            Location = r.Location.Trim(),
            Details = r.Details.Trim(),
            CreatorId = r.CreatorId,
            ReportDate = DateTime.UtcNow
        };
    }

    public override FailureReportResponse FromEntity(FailureReport e)
    {
        return new()
        {
            Id = e.Id,
            CreatorName = e.Creator.Name,
            CreatorId = e.Creator.Id,
            Details = e.Details,
            Location = e.Location,
            IsFixed = e.IsFixed,
            ReportDate = e.ReportDate,
            FixDate = e.FixDate
        };
    }

    public override FailureReport UpdateEntity(CreateFailureReportRequest r, FailureReport e)
    {
        e.Location = r.Location.Trim();
        e.Details = r.Details.Trim();

        return e;
    }
}