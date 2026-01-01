namespace API.Endpoints.FailureReports.Patch;

using System.Net;
using API.Services.Email;
using JetBrains.Annotations;
using MimeKit;
using MimeKit.Text;

public sealed record PatchFailureReportCommand : ICommand
{
    public required int ReportId { get; init; }

    public required int FixerId { get; init; }

    public required string? Note { get; init; }
}

[UsedImplicitly]
public sealed class PatchFailureReportCommandHandler(
    ILogger<PatchFailureReportCommandHandler> logger,
    IDbContextFactory<AppDbContext> dbContextFactory,
    IEmailService emailService)
    : ICommandHandler<PatchFailureReportCommand>
{
    public async Task ExecuteAsync(PatchFailureReportCommand command, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        var report = await dbContext.FailureReports.AsNoTracking()
                         .Where(x => x.Id == command.ReportId)
                         .Select(x => new
                         {
                             x.Location,
                             x.Details,
                             x.IsFixed,
                             CreatorEmail = x.Creator.Email,
                             CreatorName = x.Creator.Name
                         })
                         .FirstOrDefaultAsync(ct);

        if (report is null)
        {
            logger.LogWarning("Failure report {ReportId} not found", command.ReportId);
            return;
        }

        var fixer = await dbContext.Users.AsNoTracking()
                        .Where(x => x.Id == command.FixerId)
                        .Select(x => new { x.Email, x.Name })
                        .FirstOrDefaultAsync(ct);

        if (fixer is null)
        {
            logger.LogError("User {FixerId} not found", command.FixerId);
            return;
        }

        var message = new MimeMessage
        {
            Subject = "Gedimų žurnalas",
            To = { new MailboxAddress(report.CreatorName, report.CreatorEmail) },
            ReplyTo = { new MailboxAddress(fixer.Name, fixer.Email) },
            Body = new TextPart(TextFormat.Html)
            {
                Text =
                    $"""
                     <p>Atnaujinta informacija apie jūsų praneštą gedimą.</p>
                     <ul>
                        <li><b>Būsena:</b> Gedimas {report.IsFixed switch { true => "sutvarkytas", false => "nesutvarkytas", null => "tvarkomas" }}</li>
                        {(!string.IsNullOrWhiteSpace(command.Note) ? $"<li><b>Pastaba:</b> {WebUtility.HtmlEncode(command.Note.Trim())}</li>" : string.Empty)}
                        <li style="margin-top:1rem;"><b>Vieta:</b> {WebUtility.HtmlEncode(report.Location)}</li>
                        <li><b>Apibūdinimas:</b> {WebUtility.HtmlEncode(report.Details)}</li>
                     </ul>
                     <p style="margin-top:2.5rem;"><i>Jei turite klausimų, galite "atsakyti" tiesiai į šį el. laišką.</i></p>
                     """
            }
        };

        await emailService.SendAsync(message);
    }
}