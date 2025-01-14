using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nacos.AspNetCore.V2;
using System;
using SyZero.Nacos;
using SyZero.Service;

namespace SyZero
{
    /// <summary>
    /// Nacos配置扩展
    /// </summary>
    public static class SyZeroNacosExtension
    {
        //
        // 摘要:
        //     Add Nacos AspNet. This will register and de-register instance automatically.
        //     Mainly for nacos server 2.x
        //
        // 参数:
        //   services:
        //     services.
        //
        //   configuration:
        //     configuration
        //
        //   section:
        //     section, default is nacos
        //
        // 返回结果:
        //     IServiceCollection
        public static IServiceCollection AddNacos(this IServiceCollection services, IConfiguration configuration, string section = "nacos")
        {
            services.AddSingleton<IServiceManagement, ServiceManagement>();
            return services.AddNacosAspNet(configuration, section);
        }

        //
        // 摘要:
        //     Add Nacos AspNet. This will register and de-register instance automatically.
        //     Mainly for nacos server 2.x
        //
        // 参数:
        //   services:
        //     services
        //
        //   optionsAccs:
        //     optionsAccs
        //
        // 返回结果:
        //     IServiceCollection
        public static IServiceCollection AddNacos(this IServiceCollection services, Action<NacosAspNetOptions> options)
        {
            services.AddSingleton<IServiceManagement, ServiceManagement>();
            return services.AddNacosAspNet(options);
        }
    }
}