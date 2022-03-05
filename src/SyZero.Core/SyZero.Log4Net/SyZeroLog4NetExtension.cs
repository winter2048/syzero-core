using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Log4Net;

namespace SyZero
{
    public static class SyZeroLog4NetExtension
    {

        /// <summary>
        /// 注册Log4NetModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroLog4Net(this ContainerBuilder builder)
        {
            builder.RegisterModule<Log4NetModule>();
            return builder;
        }
    }
}
