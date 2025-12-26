using Microsoft.Extensions.Configuration;
using NConsul;
using System;
using System.Threading;
using SyZero;
using SyZero.Consul;
using SyZero.Consul.Config;
using SyZero.Service;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Consul 配置构建器扩展方法
    /// </summary>
    public static class ConsulConfigurationBuilderExtensions
    {
        /// <summary>
        /// 添加 Consul 配置源
        /// </summary>
        /// <param name="builder">配置构建器</param>
        /// <param name="serviceKey">服务键名</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="options">配置源选项</param>
        /// <returns>配置构建器</returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string serviceKey, CancellationToken cancellationToken, Action<IConsulConfigurationSource> options)
        {
            ConsulConfigurationSource consulConfigSource = new ConsulConfigurationSource(serviceKey, cancellationToken);
            options(consulConfigSource);
            return builder.Add(consulConfigSource);
        }

        /// <summary>
        /// 添加 Consul 配置源（从配置读取）
        /// </summary>
        /// <param name="builder">配置构建器</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="configuration">配置，为 null 时使用 AppConfig.Configuration</param>
        /// <param name="sectionName">配置节名称，默认为 "Consul"</param>
        /// <returns>配置构建器</returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, CancellationToken cancellationToken, IConfiguration configuration = null, string sectionName = ConsulServiceOptions.SectionName)
        {
            var config = configuration ?? AppConfig.Configuration;
            var consulOptions = new ConsulServiceOptions();
            config.GetSection(sectionName).Bind(consulOptions);
            
            return builder.AddConsul(AppConfig.ServerOptions.Name, cancellationToken, source =>
            {
                source.ConsulClientConfiguration = cco => {
                    cco.Address = new Uri(consulOptions.ConsulAddress);
                    cco.Token = consulOptions.Token;
                };
                source.Optional = true;
                source.ReloadOnChange = true;
                source.ReloadDelay = 300;
                source.QueryOptions = new QueryOptions
                {
                    WaitIndex = 0
                };
            });
        }
    }
}