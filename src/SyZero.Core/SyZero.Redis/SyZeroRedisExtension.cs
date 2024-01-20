using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyZero.Cache;
using SyZero.Redis;
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
    }
}
