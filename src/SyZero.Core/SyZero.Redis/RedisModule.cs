using Autofac;
using FreeRedis;
using SyZero.Cache;
using SyZero.Configurations;
using SyZero.Util;
using System.Linq;
using System.Collections.Generic;

namespace SyZero.Redis
{
    public class RedisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RedisOptions options = AppConfig.GetSection<RedisOptions>("Redis");
            builder.Register(
                    c =>
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
                        // var Redis = new CSRedisCache(new CSRedisClient("mymaster,password=584ab235-0e5e-11e9-a088-0c9d92bf536f,poolsize=50,connectTimeout=200,ssl=false", new string[] { "192.168.2.10:30600" }, true));
                        return redis;
                    })
                    .As<RedisClient>()
                    .InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<Cache>().As<ICache>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<LockUtil>().As<ILockUtil>().SingleInstance().PropertiesAutowired();
        }
    }
}
