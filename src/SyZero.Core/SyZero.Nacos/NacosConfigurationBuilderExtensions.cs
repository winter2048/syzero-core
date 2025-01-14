using Microsoft.Extensions.Configuration;
using Nacos.Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.Configuration
{
    public static class NacosConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            return builder.AddNacosV2Configuration(configuration.GetSection("NacosConfig"));
        }

        public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, Action<NacosV2ConfigurationSource> action)
        {
            return builder.AddNacosV2Configuration(action);
        }
    }
}
