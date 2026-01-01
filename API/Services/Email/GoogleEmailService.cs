namespace API.Services.Email;

using API.Services.Options;
using Google.Apis.Gmail.v1;
using Microsoft.Extensions.Options;
using MimeKit;

public sealed class GoogleEmailService(
    IOptions<GoogleOptions> googleOptions,
    IOptions<EmailOptions> emailOptions)
    : IEmailService
{
    private readonly GmailService _gmailService = new(new()
    {
        HttpClientInitializer = googleOptions.Value.CreateCredential()
            .CreateScoped(GmailService.ScopeConstants.GmailSend)
            .CreateWithUser(emailOptions.Value.SenderEmail)
    });

    private readonly MailboxAddress _sender = new(emailOptions.Value.SenderName,
        emailOptions.Value.SenderEmail);

    public async Task SendAsync(MimeMessage message)
    {
        message.From.Add(_sender);

        using var stream = new MemoryStream();
        await message.WriteToAsync(stream);

        var raw = Convert.ToBase64String(stream.ToArray())
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        await _gmailService.Users.Messages.Send(new() { Raw = raw }, "me").ExecuteAsync();
    }
}