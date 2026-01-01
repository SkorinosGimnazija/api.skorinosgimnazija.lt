namespace API.Extensions;

public static class DateExtensions
{
    private static readonly TimeZoneInfo LithuanianTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Vilnius");

    extension(TimeProvider timeProvider)
    {
        public DateTime UtcNow
        {
            get { return timeProvider.GetUtcNow().UtcDateTime; }
        }

        public DateTime LtNow
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(timeProvider.UtcNow, LithuanianTimeZone); }
        }
    }
}