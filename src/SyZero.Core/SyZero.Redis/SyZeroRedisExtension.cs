using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyZero.Cache;
using SyZero.Redis;
using SyZero.Service;
using SyZero.Util;

namespace SyZero
{
    public static class SyZeroRedisExtension
    {

        /// <summary>
        /// 注册RedisModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroRedis(this IServiceCollection services)
        {
            RedisOptions options = AppConfig.GetSection<RedisOptions>("Redis");
            services.AddSingleton<RedisClient>(c =>
            {
                RedisClient redis = null;
                switch (options.Type)
                {
                    case RedisType.MasterSlave:
                        var slave = new List<ConnectionStringBuilder>();
                        slave.AddRange(options.Slave.Select(p => ConnectionStringBuilder.Parse(p)).ToList());
                        redis = new RedisClient(options.Master, slave.ToArray());
                        break;
                    case RedisType.Sentinel:
                        redis = new RedisClient(
                             options.Master,
                             options.Sentinel.ToArray(),
                             true
                              );
                        break;
                    case RedisType.Cluster:
                        var clusters = new List<ConnectionStringBuilder>();
                        clusters.Add(options.Master);
                        clusters.AddRange(options.Slave.Select(p => ConnectionStringBuilder.Parse(p)).ToList());
                        redis = new RedisClient(clusters.ToArray());
                        break;
                    default:
                        System.Console.WriteLine("Redis:配置错误！！！！");
                        break;
                }
                return redis;
            });

            services.AddSingleton<ICache, Redis.Cache>();
            services.AddSingleton<ILockUtil, LockUtil>();
            return services;
        }

        /// <summary>
        /// 注册 Redis 服务管理
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">配置选项</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisServiceManagement(this IServiceCollection services, Action<RedisServiceManagementOptions> configureOptions = null)
        {
            // 确保 Redis 已注册
            var redisDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(RedisClient));
            if (redisDescriptor == null)
            {
                services.AddSyZeroRedis();
            }

            // 配置选项
            var options = AppConfig.GetSection<RedisServiceManagementOptions>(RedisServiceManagementOptions.SectionName)
                          ?? new RedisServiceManagementOptions();
            configureOptions?.Invoke(options);
            options.Validate();

            services.AddSingleton(options);
            services.AddSingleton<IServiceManagement, RedisServiceManagement>();

            return services;
        }

        /// <summary>
        /// 注册 Redis 服务管理（使用配置节）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="sectionName">配置节名称</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisServiceManagement(this IServiceCollection services, string sectionName)
        {
            // 确保 Redis 已注册
            var redisDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(RedisClient));
            if (redisDescriptor == null)
            {
                services.AddSyZeroRedis();
            }

            // 从配置节读取选项
            var options = AppConfig.GetSection<RedisServiceManagementOptions>(sectionName)
                          ?? new RedisServiceManagementOptions();
            options.Validate();

            services.AddSingleton(options);
            services.AddSingleton<IServiceManagement, RedisServiceManagement>();

            return services;
        }
    }
}
