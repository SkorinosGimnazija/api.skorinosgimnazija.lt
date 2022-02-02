namespace SkorinosGimnazija.Infrastructure.Email;

using Application.Common.Interfaces;
using Calendar;
using Domain.Options;
using Extensions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly GmailService _gmailService;
    private readonly MailboxAddress _sender;

    public EmailService(
        IOptions<GoogleOptions> googleOptions,
        IOptions<EmailOptions> emailOptions)
    {
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

        await _gmailService.Users.Messages.Send(message.ToGmail(), "me").ExecuteAsync();
    }
}