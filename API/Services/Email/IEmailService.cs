namespace API.Services.Email;

using MimeKit;

public interface IEmailService
{
    Task SendAsync(MimeMessage message);
}