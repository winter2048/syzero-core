using Microsoft.Extensions.DependencyInjection;
using NConsul;
using NConsul.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Consul;
using SyZero.Consul.Config;
using SyZero.Service;

namespace SyZero
{
    public static class SyZeroConsulExtension
    {
        /// <summary>
        /// 注册IServiceManagement
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            // 获取服务配置项
            var consulOptions = AppConfig.GetSection<ConsulServiceOptions>("Consul");
            services.AddSingleton<IConsulClient>(p =>
            {
                return new ConsulClient(config =>
                {
                    config.Address = new Uri(consulOptions.ConsulAddress);
                });
            });
            services.AddSingleton<IServiceManagement, ServiceManagement>();
            return services;
        }
    }
}
