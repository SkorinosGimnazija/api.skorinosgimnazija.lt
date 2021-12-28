namespace SkorinosGimnazija.Application.BullyReports.Events;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using Infrastructure.Email;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;

public record BullyReportCreatedNotification(int Id) : INotification;

public class BullyReportCreatedNotificationHandler : INotificationHandler<BullyReportCreatedNotification>
{
    private readonly ILogger<BullyReportCreatedNotificationHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmployeeService _employeeService;
    private readonly string _groupId;
    private readonly string _baseUrl;

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
        _groupId = groupOptions.Value.BullyManagers;
        _baseUrl = urlOptions.Value.Admin;
    }


    public async Task Handle(BullyReportCreatedNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            var groupEmail = await _employeeService.GetGroupEmailAsync(_groupId);

            var body = @$"<p>Gautas <a href=""{_baseUrl}/bullies/{notification.Id}"">naujas pranešimas</a>.</p>";

            await _emailService.SendAsync(groupEmail, "Patyčių dėžutė", body);
        }
        catch (Exception e)
        {
            _logger.LogError(0, e, "Email notification failed");
        }
    }
}
