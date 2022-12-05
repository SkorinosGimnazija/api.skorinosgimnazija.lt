namespace SkorinosGimnazija.Application.TechJournal.Notifications;

using Common.Interfaces;
using Domain.Entities.TechReports;
using Domain.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    private readonly IAppDbContext _context;

    public TechJournalReportCreatedNotificationHandler(
        ILogger<TechJournalReportCreatedNotificationHandler> logger,
        IAppDbContext context,
        IEmailService emailService,
        IEmployeeService employeeService,
        IOptions<GroupOptions> groupOptions,
        IOptions<UrlOptions> urlOptions)
    {
        _logger = logger;
        _context = context;
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
            var teacher = await _context.Users.FirstAsync(x => x.Id == notification.Report.UserId);

            var reportLink = $"{_baseUrl}/teacher/failures/{notification.Report.Id}";
            var body = @$"
                    <p>Gautas naujas pranešimas apie <a href=""{reportLink}"">gedimą</a>.</p>
                    <ul style=""margin-top:20px"">
                        <li><b>Mokytojas</b>: {teacher.DisplayName}</li>
                        <li><b>Vieta</b>: {notification.Report.Place}</li>
                        <li><b>Apibūdinimas</b>: {notification.Report.Details}</li>
                    </ul>";

            await _emailService.SendAsync(groupEmail, "Gedimų žurnalas", body);
        }
        catch (Exception e)
        {
            _logger.LogError(0, e, "Email notification failed");
        }
    }
}