﻿namespace SkorinosGimnazija.Infrastructure.Email;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody);
}