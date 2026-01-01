namespace API.Endpoints.BullyReports.Patch;

using API.Database.Entities.BullyReports;

public sealed class PatchBullyReportMapper
    : RequestMapper<PatchBullyReportRequest, BullyReport>
{
    public override BullyReport UpdateEntity(PatchBullyReportRequest r, BullyReport e)
    {
        e.Actions = string.IsNullOrWhiteSpace(r.Actions) ? null : r.Actions.Trim();

        return e;
    }
}