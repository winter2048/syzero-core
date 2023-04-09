using Autofac;
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
        public static ContainerBuilder AddConsul(this ContainerBuilder builder)
        {
            // 获取服务配置项
            var consulOptions = AppConfig.GetSection<ConsulServiceOptions>("Consul");
            builder.Register(p =>
            {
                return new ConsulClient(config =>
                {
                    config.Address = new Uri(consulOptions.ConsulAddress);
                });
            }).As<IConsulClient>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<ServiceManagement>().As<IServiceManagement>().SingleInstance().PropertiesAutowired();
            return builder;
        }
    }
}
