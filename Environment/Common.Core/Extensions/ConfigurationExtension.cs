using Microsoft.Extensions.Configuration;

namespace Common.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetServerConfiguration(
            this IConfiguration configuration,
            string name)
        {
            return configuration?.GetSection("ServerConfiguration")?[name];
        }

        public static string GetServiceRoutesConfiguration(
            this IConfiguration configuration,
            string name)
        {
            return configuration?.GetSection("ServiceRoutes")?[name];
        }
    }
}