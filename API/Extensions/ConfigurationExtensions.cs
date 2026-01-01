namespace API.Extensions;

using Npgsql;

public static class ConfigurationExtensions
{
    extension(IConfiguration config)
    {
        public string GetNpgsqlConnectionString(string key)
        {
            var uri = new Uri(config[key]!);

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

    extension(IServiceCollection services)
    {
        public void AddOptionsAndValidate<TOptions>(string key)
            where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(key)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}