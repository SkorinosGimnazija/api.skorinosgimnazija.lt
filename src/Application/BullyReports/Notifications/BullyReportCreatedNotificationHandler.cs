namespace SkorinosGimnazija.Application.BullyReports.Notifications;

using Common.Interfaces;
using Domain.Entities.Bullies;
using Domain.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public record BullyReportCreatedNotification(BullyReport Report) : INotification;

public class BullyReportCreatedNotificationHandler : INotificationHandler<BullyReportCreatedNotification>
{
    private readonly string _baseUrl;
    private readonly IEmailService _emailService;
    private readonly IEmployeeService _employeeService;
    private readonly string _groupId;
    private readonly ILogger<BullyReportCreatedNotificationHandler> _logger;

    public BullyReportCreatedNotificationHandler(
        ILogger<BullyReportCreatedNotificationHandler> logger,
        IEmailService emailService,
        IEmployeeService employeeService,
        IOptions<GroupOptions> groupOptions,
        IOptions<UrlOptions> urlOptions)
    {
        _logger = logger;
        _emailService = emailService;
        _employeeService = employeeService;
        _groupId = groupOptions.Value.SocialNotifications;
        _baseUrl = urlOptions.Value.Admin;
    }

    public async Task Handle(BullyReportCreatedNotification notification, CancellationToken _)
    {
        try
        {
            var groupEmail = await _employeeService.GetGroupEmailAsync(_groupId);

            var reportLink = $"{_baseUrl}/admin/bullies/{notification.Report.Id}";
            var body = @$"<p>Gautas <a href=""{reportLink}"">naujas pranešimas</a>.</p>";

            await _emailService.SendAsync(groupEmail, "Patyčių dėžutė", body);
        }
        catch (Exception e)
        {
            _logger.LogError(0, e, "Email notification failed");
        }
    }
}