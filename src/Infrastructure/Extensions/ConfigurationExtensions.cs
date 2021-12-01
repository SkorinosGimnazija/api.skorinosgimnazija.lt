namespace SkorinosGimnazija.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Npgsql;

public static class ConfigurationExtensions
{
    public static string GetNpgsqlConnectionString(this IConfiguration config, string key)
    {
        var uri = new Uri(config[key]);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port,
            Database = uri.Segments[1],
            Username = uri.UserInfo.Split(":")[0],
            Password = uri.UserInfo.Split(":")[1]
        };

        return builder.ConnectionString;
    }
}