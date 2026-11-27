namespace API.Extensions;

public static class DateExtensions
{
    extension(TimeProvider timeProvider)
    {
        public DateTime UtcNow
        {
            get { return timeProvider.GetUtcNow().DateTime; }
        }

        public DateTime LocalNow
        {
            get { return timeProvider.GetLocalNow().DateTime; }
        }
    }
}