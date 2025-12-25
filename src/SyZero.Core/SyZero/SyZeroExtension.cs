using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using SyZero.Dependency;
using SyZero.Runtime.Session;
using SyZero.Serialization;
using SyZero.Util;

namespace SyZero
{
    public static class SyZeroExtension
    {
        public static IHost UseSyZero(this IHost app)
        {
            #region Autofac依赖注入服务
            SyZeroUtil.ServiceProvider = app.Services;
            #endregion
            return app;
        }

        public static IHostApplicationBuilder AddSyZero(this IHostApplicationBuilder builder)
        {
            AppConfig.Configuration = builder.Configuration;
            builder.Services.AddSyZero();
            return builder;
        }

        /// <summary>
        /// SyZeroModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZero(this IServiceCollection services)
        {
            System.Console.WriteLine(@" _____  __    __  ______  _____   _____    _____  
/  ___/ \ \  / / |___  / | ____| |  _  \  /  _  \ 
| |___   \ \/ /     / /  | |__   | |_| |  | | | | 
\___  \   \  /     / /   |  __|  |  _  /  | | | | 
 ___| |   / /     / /__  | |___  | | \ \  | |_| | 
/_____/  /_/     /_____| |_____| |_|  \_\ \_____/ ");
            // 获取当前程序集
            System.Console.WriteLine("");
            System.Console.WriteLine("版本号: " + Assembly.GetExecutingAssembly().GetName().Version);
            System.Console.WriteLine("启动中......");

            //注入SySession
            services.AddScoped<ISySession, SySession>();

            //// 获取所有实现了IScopedDependency接口的类型
            //var scopedDependency = ReflectionHelper.GetTypes()
            //                    .Where(t => t.IsClass && !t.IsAbstract && typeof(IScopedDependency).IsAssignableFrom(t));
            //foreach (var type in scopedDependency)
            //{
            //    services.AddScoped(type.GetInterfaces().Last(), type);
            //}

            //// 获取所有实现了ISingletonDependency接口的类型
            //var singletonDependencyTypes = ReflectionHelper.GetTypes()
            //                    .Where(t => t.IsClass && !t.IsAbstract && typeof(ISingletonDependency).IsAssignableFrom(t));
            //foreach (var type in singletonDependencyTypes)
            //{
            //    services.AddSingleton(type.GetInterfaces().Last(), type);
            //}

            //// 获取所有实现了ITransientDependency接口的类型
            //var transientDependency = ReflectionHelper.GetTypes()
            //                    .Where(t => t.IsClass && !t.IsAbstract && typeof(ITransientDependency).IsAssignableFrom(t));
            //foreach (var type in transientDependency)
            //{
            //    services.AddScoped(type.GetInterfaces().Last(), type);
            //}
            services.AddClassesAsImplementedInterface(typeof(IScopedDependency), ServiceLifetime.Scoped);
            services.AddClassesAsImplementedInterface(typeof(ISingletonDependency), ServiceLifetime.Singleton);
            services.AddClassesAsImplementedInterface(typeof(ITransientDependency), ServiceLifetime.Transient);

            // 获取所有继承SyZeroServiceBase类的类型
            var syZeroServiceBaseTypes = ReflectionHelper.GetTypes()
                                .Where(t => t.IsClass && !t.IsAbstract && typeof(SyZeroServiceBase).IsAssignableFrom(t));
            foreach (var type in syZeroServiceBaseTypes)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}
