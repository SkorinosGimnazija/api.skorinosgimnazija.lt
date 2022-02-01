namespace SkorinosGimnazija.Infrastructure.Calendar;

using System.Text;

public record GoogleOptions
{
    private readonly string _credential = default!;

    public string ClientId { get; init; } = default!;

    public string Admin { get; init; } = default!;

    public string Credential
    {
        get { return _credential; }
        init { _credential = Encoding.UTF8.GetString(Convert.FromBase64String(value)); }
    }
}