using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using SyZero.AspNetCore;
using SyZero.AspNetCore.Controllers;
using SyZero.AspNetCore.Middleware;
using SyZero.Dependency;

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
        public static IServiceCollection AddSyZeroController(this IServiceCollection services)
        {
            services.AddScoped<SyAuthMiddleware>();

            // 获取所有实现了SyZeroController的类型
            var transientDependency = ReflectionHelper.GetTypes()
                                .Where(t => t.IsClass && typeof(SyZeroController).IsAssignableFrom(t));
            foreach (var type in transientDependency)
            {
                services.AddScoped(type);
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
    }
}
