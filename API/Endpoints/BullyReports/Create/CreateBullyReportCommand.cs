namespace API.Endpoints.BullyReports.Create;

using System.Net;
using API.Services.Email;
using API.Services.Options;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

public sealed record CreateBullyReportCommand : ICommand
{
    public required int ReportId { get; init; }
}

[UsedImplicitly]
public sealed class CreateBullyReportCommandHandler(
    ILogger<CreateBullyReportCommandHandler> logger,
    IDbContextFactory<AppDbContext> dbContextFactory,
    IOptions<NotificationOptions> notificationOptions,
    IOptions<UrlOptions> urlOptions,
    IEmailService emailService)
    : ICommandHandler<CreateBullyReportCommand>
{
    public async Task ExecuteAsync(CreateBullyReportCommand command, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);

        var report = await dbContext.BullyReports.AsNoTracking()
                         .Where(x => x.Id == command.ReportId)
                         .Select(x => new
                         {
                             x.IsPublicReport,
                             x.VictimName,
                             x.BullyName,
                             x.Details
                         })
                         .FirstOrDefaultAsync(ct);

        if (report is null)
        {
            logger.LogWarning("Bully report {ReportId} not found", command.ReportId);
            return;
        }

        using var message = new MimeMessage();

        message.Subject = report.IsPublicReport ? "Patyčių dėžutė" : "Patyčių žurnalas";
        message.To.Add(new MailboxAddress(null, notificationOptions.Value.SocialEmail));
        message.Body = new TextPart(TextFormat.Html)
        {
            Text =
                $"""
                 <p>Gautas naujas pranešimas apie <a href="{urlOptions.Value.Admin}/admin/bullies/{command.ReportId}/resolve">patyčias</a>.</p>
                 <ul>
                    <li><b>Auka:</b> {WebUtility.HtmlEncode(report.VictimName)}</li>
                    <li><b>Skriaudėjas:</b> {WebUtility.HtmlEncode(report.BullyName)}</li>
                    <li><b>Patyčios:</b> {WebUtility.HtmlEncode(report.Details)}</li>
                 </ul>
                 """
        };

        await emailService.SendAsync(message);
    }
}