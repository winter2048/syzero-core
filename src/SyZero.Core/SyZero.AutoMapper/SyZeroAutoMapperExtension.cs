using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SyZero.AutoMapper;

namespace SyZero
{
    public static class SyZeroAutoMapperExtension
    {

        /// <summary>
        /// AutoMapperModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZeroAutoMapper(this ContainerBuilder builder)
        {
            builder.RegisterModule<AutoMapperModule>();
            return builder;
        }
    }
}
