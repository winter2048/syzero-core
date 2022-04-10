using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NConsul;
using System;
using System.Collections.Generic;
using SyZero;
using SyZero.Consul.Config;

namespace Microsoft.AspNetCore.Builder
{
    // consul服务注册扩展类
    public static class ConsulRegistrationExtensions
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            Console.WriteLine("注入consul...");
            // 获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            // 获取服务配置项
            var consulOptions = AppConfig.GetSection<ConsulServiceOptions>("Consul"); ;

            // 获取App配置项
            var serverOptions = AppConfig.ServerOptions;

            // 服务ID必须保证唯一
            consulOptions.ServiceId = $"{serverOptions.Name}{serverOptions.WanIp}{serverOptions.Port}".Replace(".", "").ToLower();

            var consulClient = new ConsulClient(configuration =>
            {
                //服务注册的地址，集群中任意一个地址
                configuration.Address = new Uri(consulOptions.ConsulAddress);
            });

            var Check = new AgentServiceCheck
            {
                // 注册超时
                Timeout = TimeSpan.FromSeconds(1),
                // 服务停止多久后注销服务
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(1),
                // 健康检查时间间隔
                Interval = TimeSpan.FromSeconds(serverOptions.InspectInterval)
            };

            //健康检查地址
            if (serverOptions.Protocol == SyZero.Runtime.Security.ProtocolType.GRPC)
            {
                Check.GRPC = $"{serverOptions.WanIp}:{serverOptions.Port}";
                Check.GRPCUseTLS = false;
            }
            else
            {
                Check.HTTP = $"{serverOptions.Protocol}://{serverOptions.WanIp}:{serverOptions.Port}{consulOptions.HealthCheck}";
            }

            // 节点服务注册对象
            var registration = new AgentServiceRegistration()
            {
                ID = consulOptions.ServiceId,
                Name = serverOptions.Name, // 服务名
                Address = serverOptions.WanIp,
                Port = serverOptions.Port.ToInt32(), // 服务端口
                Check = Check,
                Meta = new Dictionary<string, string>
                {
                    { "Protocol", serverOptions.Protocol.ToString() }
                }
            };

            // 注册服务
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 应用程序终止时，注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(consulOptions.ServiceId).Wait();
            });

            return app;
        }
    }
}
