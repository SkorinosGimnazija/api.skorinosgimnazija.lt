namespace API.Utils
{
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.Configuration;

    public class ConfigUtils
    {
        private readonly IConfiguration _configuration;

        public ConfigUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string[] GetCorsOrigins()
        {
            return _configuration["CORS_ORIGINS"].Split(';');
        }

        public string GetDatabaseConnectionString()
        {
            var url = _configuration["DATABASE_URL"];
            var regex = new Regex("^(?<protocol>.*)://(?<username>.*):(?<password>.*)@(?<host>.*):(?<port>.*)/(?<database>.*)$");
            var data = regex.Matches(url)[0].Groups;

            return
                $"Host={data["host"]};Port={data["port"]};Database={data["database"]};Username={data["username"]};Password={data["password"]}";
        }

        public string GetGoogleClientId()
        {
            return _configuration["GOOGLE_CLIENT_ID"];
        }

        public string GetGoogleClientSecret()
        {
            return _configuration["GOOGLE_CLIENT_SECRET"];
        }
    }
}