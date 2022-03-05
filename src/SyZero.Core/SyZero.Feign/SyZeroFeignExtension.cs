using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Feign;

namespace SyZero
{
    public static class SyZeroFeignExtension
    {

        /// <summary>
        /// 注册FeignModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroFeign(this ContainerBuilder builder)
        {
            builder.RegisterModule<FeignModule>();
            return builder;
        }
    }
}
