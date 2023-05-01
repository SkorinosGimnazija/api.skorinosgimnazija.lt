namespace SkorinosGimnazija.Application.TechJournal.Notifications;

using Common.Interfaces;
using Domain.Entities.TechReports;
using Domain.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public record TechJournalReportPatchedNotification(TechJournalReport Report, bool? OldFixedState) : INotification;

public class TechJournalReportPatchedNotificationHandler : INotificationHandler<TechJournalReportPatchedNotification>
{
    private readonly string _baseUrl;
    private readonly IEmailService _emailService;
    private readonly IEmployeeService _employeeService;
    private readonly string _groupId;
    private readonly ILogger<TechJournalReportCreatedNotificationHandler> _logger;

    public TechJournalReportPatchedNotificationHandler(
        ILogger<TechJournalReportCreatedNotificationHandler> logger,
        IEmailService emailService,
        IEmployeeService employeeService,
        IOptions<GroupOptions> groupOptions,
        IOptions<UrlOptions> urlOptions)
    {
        _logger = logger;
        _emailService = emailService;
        _employeeService = employeeService;
        _groupId = groupOptions.Value.TechStatusNotifications;
        _baseUrl = urlOptions.Value.Admin;
    }

    public async Task Handle(TechJournalReportPatchedNotification notification, CancellationToken _)
    {
        if (notification.Report.IsFixed != false || notification.Report.IsFixed == notification.OldFixedState)
        {
            return;
        }

        try
        {
            var groupEmail = await _employeeService.GetGroupEmailAsync(_groupId);

            var reportLink = $"{_baseUrl}/teacher/failures/{notification.Report.Id}/fix";
            var body = @$"
                    <p><a href=""{reportLink}"">Gedimas</a> negali būti sutvarkytas.</p>
                    <ul style=""margin-top:20px"">
                        <li><b>Vieta</b>: {notification.Report.Place}</li>
                        <li><b>Apibūdinimas</b>: {notification.Report.Details}</li>
                        <li><b>Pastabos</b>: {notification.Report.Notes}</li>
                    </ul>";

            await _emailService.SendAsync(groupEmail, "Gedimų žurnalas", body);
        }
        catch (Exception e)
        {
            _logger.LogError(0, e, "Email notification failed");
        }
    }
}