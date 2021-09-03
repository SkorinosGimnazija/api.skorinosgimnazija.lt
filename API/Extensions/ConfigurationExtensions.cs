
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace API.Extensions;
public static class ConfigurationExtensions
{
    public static string GetDatabaseConnectionString(this IConfiguration config)
    {
        var url = config["DATABASE_URL"];
        var regex = new Regex("^(?<protocol>.*)://(?<username>.*):(?<password>.*)@(?<host>.*):(?<port>.*)/(?<database>.*)$");
        var data = regex.Matches(url)[0].Groups;

        return
            $"Host={data["host"]};Port={data["port"]};Database={data["database"]};Username={data["username"]};Password={data["password"]}";
    }

    public static string[] GetCorsOrigins(this IConfiguration config)
    {
        return config["CORS_ORIGINS"].Split(';');
    }

    public static string GetGoogleClientId(this IConfiguration config)
    {
        return config["GOOGLE_CLIENT_ID"];
    }

    public static string GetGoogleClientSecret(this IConfiguration config)
    {
        return config["GOOGLE_CLIENT_SECRET"];
    }

}
