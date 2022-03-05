using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using SyZero.Util;

namespace SyZero
{
    public static class SyZeroExtension
    {
        public static IApplicationBuilder UseSyZero(this IApplicationBuilder app)
        {
            #region Autofac依赖注入服务
            AutofacUtil.Container = app.ApplicationServices.GetAutofacRoot();
            #endregion
            return app;
        }

        /// <summary>
        /// SyZeroModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSyZero(this ContainerBuilder builder)
        {
            builder.RegisterModule<SyZeroModule>();
            return builder;
        }

        public static AutofacServiceProvider AddSyZeroAutofac(this IServiceCollection service, Action<ContainerBuilder> action)
        {
            var builder = new ContainerBuilder();//实例化 AutoFac  容器            
            builder.Populate(service);

            action.Invoke(builder);

            IContainer ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }
    }
}
