using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.Web.Common;

namespace SyZero
{
    public static class SyZeroCommonExtension
    {

        /// <summary>
        /// 注册CommonModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroCommon(this ContainerBuilder builder)
        {
            builder.RegisterModule<CommonModule>();
            return builder;
        }
    }
}
