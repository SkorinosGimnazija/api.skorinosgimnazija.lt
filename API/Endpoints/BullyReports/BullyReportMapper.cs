namespace API.Endpoints.BullyReports;

using API.Database.Entities.BullyReports;
using API.Endpoints.BullyReports.Create;
using API.Endpoints.BullyReports.Public;
using API.Endpoints.BullyReports.Update;

public sealed class BullyReportMapper
    : Mapper<CreateBullyReportRequest, BullyReportResponse, BullyReport>
{
    public override BullyReport ToEntity(CreateBullyReportRequest r)
    {
        return new()
        {
            IsPublicReport = false,
            CreatedAt = DateTime.UtcNow,
            Date = r.Date,
            VictimName = r.VictimName.Trim(),
            BullyName = r.BullyName.Trim(),
            Location = r.Location.Trim(),
            Details = r.Details.Trim(),
            Actions = string.IsNullOrWhiteSpace(r.Actions) ? null : r.Actions.Trim(),
            CreatorId = r.CreatorId
        };
    }

    public BullyReport ToEntity(CreateBullyReportPublicRequest r)
    {
        return new()
        {
            IsPublicReport = true,
            CreatedAt = DateTime.UtcNow,
            Date = r.Date,
            VictimName = r.VictimName.Trim(),
            BullyName = r.BullyName.Trim(),
            Location = r.Location.Trim(),
            Details = r.Details.Trim(),
            Observers = string.IsNullOrWhiteSpace(r.Observers) ? null : r.Observers.Trim()
        };
    }

    public override BullyReportResponse FromEntity(BullyReport e)
    {
        return new()
        {
            Id = e.Id,
            IsPublicReport = e.IsPublicReport,
            CreatedAt = e.CreatedAt,
            Date = e.Date,
            VictimName = e.VictimName,
            BullyName = e.BullyName,
            Location = e.Location,
            Details = e.Details,
            Observers = e.Observers,
            Actions = e.Actions,
            CreatorId = e.CreatorId,
            CreatorName = e.Creator?.Name
        };
    }

    public BullyReport UpdateEntity(UpdateBullyReportRequest r, BullyReport e)
    {
        e.Date = r.Date;
        e.VictimName = r.VictimName.Trim();
        e.BullyName = r.BullyName.Trim();
        e.Location = r.Location.Trim();
        e.Details = r.Details.Trim();
        e.Observers = string.IsNullOrWhiteSpace(r.Observers) ? null : r.Observers.Trim();
        e.Actions = string.IsNullOrWhiteSpace(r.Actions) ? null : r.Actions.Trim();

        return e;
    }
}