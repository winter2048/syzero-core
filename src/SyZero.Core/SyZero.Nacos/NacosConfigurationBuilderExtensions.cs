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
using Nacos.Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using SyZero.Nacos.Config;

namespace SyZero.Nacos
{
    /// <summary>
    /// Nacos配置扩展
    /// </summary>
    public static class NacosConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, CancellationToken cancellationToken, Action<NacosConfigurationSource> options)
        {
            return builder.AddNacosConfiguration(options);
        }

        public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, CancellationToken cancellationToken)
        {
            // 获取服务配置项
            var nacosOptions = AppConfig.GetSection<NacosServiceOptions>("Nacos");
            return builder.AddNacos(cancellationToken, source =>
           {
               source.DataId = AppConfig.ServerOptions.Name;
               source.Group = "";
               source.Tenant = "";
               source.Optional = false;
               source.ServerAddresses = nacosOptions.NacosAddresses;
           });
        }
    }
}