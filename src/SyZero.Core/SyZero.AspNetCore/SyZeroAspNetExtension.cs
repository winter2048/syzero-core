using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.AspNetCore;

namespace SyZero
{
    public static class SyZeroAspNetExtension
    {

        /// <summary>
        /// 注册SyZeroControllerModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroController(this ContainerBuilder builder)
        {
            builder.RegisterModule<SyZeroControllerModule>();
            return builder;
        }
    }
}
