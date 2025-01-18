// *****************************************************************************************************************
// Project          : Navyblue
// File             : ConsulConfigurationBuilderExtensions.cs
// Created          : 2019-05-23  19:30
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-05-24  11:42
// *****************************************************************************************************************
// <copyright file="ConsulConfigurationBuilderExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

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
    /// </summary>
    public static class ConsulConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string serviceKey, CancellationToken cancellationToken, Action<IConsulConfigurationSource> options)
        {
            ConsulConfigurationSource consulConfigSource = new ConsulConfigurationSource(serviceKey, cancellationToken);
            options(consulConfigSource);
            return builder.Add(consulConfigSource);
        }

        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, CancellationToken cancellationToken)
        {
            // 获取服务配置项
            var consulOptions = AppConfig.GetSection<ConsulServiceOptions>("Consul");
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