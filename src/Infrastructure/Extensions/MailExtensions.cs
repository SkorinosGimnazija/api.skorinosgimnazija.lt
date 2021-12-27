namespace SkorinosGimnazija.Infrastructure.Extensions;

using Google.Apis.Gmail.v1.Data;
using MimeKit;

public static class MailExtensions
{
    public static Message ToGmail(this MimeMessage message)
    {
        using var stream = new MemoryStream();
        message.WriteTo(stream);

        var raw = Convert.ToBase64String(stream.GetBuffer())
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        return new() { Raw = raw };
    }
}