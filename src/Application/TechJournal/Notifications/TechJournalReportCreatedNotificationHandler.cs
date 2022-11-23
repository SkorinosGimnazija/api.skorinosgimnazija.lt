namespace SkorinosGimnazija.Application.TechJournal.Notifications;

using Common.Interfaces;
using Domain.Entities.TechReports;
using Domain.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public record TechJournalReportCreatedNotification(TechJournalReport Report) : INotification;

public class TechJournalReportCreatedNotificationHandler : INotificationHandler<TechJournalReportCreatedNotification>
{
    private readonly string _baseUrl;
    private readonly IEmailService _emailService;
    private readonly IEmployeeService _employeeService;
    private readonly string _groupId;
    private readonly ILogger<TechJournalReportCreatedNotificationHandler> _logger;

    public TechJournalReportCreatedNotificationHandler(
        ILogger<TechJournalReportCreatedNotificationHandler> logger,
        IEmailService emailService,
        IEmployeeService employeeService,
        IOptions<GroupOptions> groupOptions,
        IOptions<UrlOptions> urlOptions)
    {
        _logger = logger;
        _emailService = emailService;
        _employeeService = employeeService;
        _groupId = groupOptions.Value.TechNotifications;
        _baseUrl = urlOptions.Value.Admin;
    }

    public async Task Handle(TechJournalReportCreatedNotification notification, CancellationToken _)
    {
        try
        {
            var groupEmail = await _employeeService.GetGroupEmailAsync(_groupId);

            var reportLink = $"{_baseUrl}/teacher/failures/{notification.Report.Id}";
            var body = @$"<p>Gautas <a href=""{reportLink}"">naujas pranešimas</a>.</p>";

            await _emailService.SendAsync(groupEmail, "Gedimų žurnalas", body);
        }
        catch (Exception e)
        {
            _logger.LogError(0, e, "Email notification failed");
        }
    }
}