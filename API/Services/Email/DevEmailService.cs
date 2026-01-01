namespace API.Services.Email;

using MimeKit;

public class DevEmailService(ILogger<DevEmailService> logger) : IEmailService
{
    public Task SendAsync(MimeMessage message)
    {
        logger.LogInformation("Email sent {email}", message.ToString());
        return Task.CompletedTask;
    }
}