using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Redis;

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
        public static ContainerBuilder AddSyZeroRedis(this ContainerBuilder builder)
        {
            builder.RegisterModule<RedisModule>();
            return builder;
        }
    }
}
