namespace API.Endpoints.FailureReports.Create;

using System.Net;
using API.Services.Email;
using API.Services.Options;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

public sealed record CreateFailureReportCommand : ICommand
{
    public required int ReportId { get; init; }
}

[UsedImplicitly]
public sealed class CreateFailureReportCommandHandler(
    ILogger<CreateFailureReportCommandHandler> logger,
    IDbContextFactory<AppDbContext> dbContextFactory,
    IOptions<NotificationOptions> notificationOptions,
    IOptions<UrlOptions> urlOptions,
    IEmailService emailService)
    : ICommandHandler<CreateFailureReportCommand>
{
    public async Task ExecuteAsync(CreateFailureReportCommand command, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        var report = await dbContext.FailureReports.AsNoTracking()
                         .Where(x => x.Id == command.ReportId)
                         .Select(x => new
                         {
                             x.Location,
                             x.Details,
                             CreatorEmail = x.Creator.Email,
                             CreatorName = x.Creator.Name
                         })
                         .FirstOrDefaultAsync(ct);

        if (report is null)
        {
            logger.LogWarning("Failure report {ReportId} not found", command.ReportId);
            return;
        }

        using var message = new MimeMessage();

        message.Subject = "Gedimų žurnalas";
        message.To.Add(new MailboxAddress(null, notificationOptions.Value.TechEmail));
        message.ReplyTo.Add(new MailboxAddress(report.CreatorName, report.CreatorEmail));
        message.Body = new TextPart(TextFormat.Html)
        {
            Text =
                $"""
                 <p>Gautas naujas pranešimas apie <a href="{urlOptions.Value.Admin}/teacher/failures/{command.ReportId}/fix">gedimą</a>.</p>
                 <ul>
                    <li><b>Mokytojas:</b> {report.CreatorName}</li>
                    <li><b>Vieta:</b> {WebUtility.HtmlEncode(report.Location)}</li>
                    <li><b>Apibūdinimas:</b> {WebUtility.HtmlEncode(report.Details)}</li>
                 </ul>
                 <p style="margin-top:2.5rem;"><i>Jei turite klausimų, galite "atsakyti" tiesiai į šį el. laišką.</i></p>
                 """
        };

        await emailService.SendAsync(message);
    }
}