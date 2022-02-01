namespace SkorinosGimnazija.Application.Common.Interfaces;

public interface ICaptchaService
{
    Task<bool> ValidateAsync(string token);
}