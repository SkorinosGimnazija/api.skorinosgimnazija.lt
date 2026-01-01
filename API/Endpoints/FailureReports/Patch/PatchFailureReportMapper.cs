namespace API.Endpoints.FailureReports.Patch;

using API.Database.Entities.FailureReports;

public sealed class PatchFailureReportMapper
    : RequestMapper<PatchFailureReportRequest, FailureReport>
{
    public override FailureReport UpdateEntity(PatchFailureReportRequest r, FailureReport e)
    {
        e.IsFixed = r.IsFixed;
        e.FixDate = DateTime.UtcNow;

        return e;
    }
}