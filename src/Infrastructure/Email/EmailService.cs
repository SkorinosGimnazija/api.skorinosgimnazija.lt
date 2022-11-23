namespace SkorinosGimnazija.Infrastructure.Email;

using Application.Common.Interfaces;
using Domain.Options;
using Extensions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly IWebHostEnvironment _env;
    private readonly GmailService _gmailService;
    private readonly ILogger<EmailService> _logger;
    private readonly MailboxAddress _sender;

    public EmailService(
        IOptions<GoogleOptions> googleOptions,
        IOptions<EmailOptions> emailOptions,
        ILogger<EmailService> logger,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
        _sender = new(emailOptions.Value.SenderName, emailOptions.Value.SenderEmail);
        _gmailService = new(new()
        {
            HttpClientInitializer = GoogleCredential.FromJson(googleOptions.Value.Credential)
                .CreateScoped(GmailService.ScopeConstants.GmailSend)
                .CreateWithUser(emailOptions.Value.SenderEmail)
        });
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        var message = new MimeMessage
        {
            Subject = subject,
            From = { _sender },
            To = { new MailboxAddress(null, to) },
            Body = new TextPart("html")
            {
                Text = htmlBody
            }
        };

        if (_env.IsProduction())
        {
            await _gmailService.Users.Messages.Send(message.ToGmail(), "me").ExecuteAsync();
        }

        _logger.LogInformation("Email sent to {email}", to);
    }
}