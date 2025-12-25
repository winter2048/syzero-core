using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SyZero.DynamicWebApi
{
    /// <summary>
    /// Dynamic WebApi 服务扩展方法
    /// </summary>
    public static class DynamicApiServiceExtensions
    {
        /// <summary>
        /// 添加 Dynamic WebApi 到依赖注入容器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="options">配置选项</param>
        /// <returns>服务集合</returns>
        /// <exception cref="ArgumentNullException">options 为 null 时抛出</exception>
        /// <exception cref="InvalidOperationException">在 AddMvc 之前调用时抛出</exception>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, DynamicWebApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Validate();

            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" 必须在 \"AddMvc\" 之后调用。");
            }

            // 注册程序集
            foreach (var assembly in ReflectionHelper.GetAssemblies())
            {
                partManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            // 注册控制器特性提供程序
            partManager.FeatureProviders.Add(new DynamicWebApiControllerFeatureProvider());

            // 注册 MVC 约定
            services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.Conventions.Add(new DynamicWebApiConvention(options));
            });

            return services;
        }

        /// <summary>
        /// 添加 Dynamic WebApi 到依赖注入容器（使用默认配置）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services)
        {
            return AddDynamicWebApi(services, new DynamicWebApiOptions());
        }

        /// <summary>
        /// 添加 Dynamic WebApi 到依赖注入容器（使用配置委托）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="optionsAction">配置委托</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, Action<DynamicWebApiOptions> optionsAction)
        {
            var options = new DynamicWebApiOptions();
            optionsAction?.Invoke(options);
            return AddDynamicWebApi(services, options);
        }
    }
}