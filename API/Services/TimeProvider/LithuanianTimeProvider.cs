namespace API.Services.TimeProvider;

public sealed class LithuanianTimeProvider : System.TimeProvider
{
    public override TimeZoneInfo LocalTimeZone { get; } =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Vilnius");
}