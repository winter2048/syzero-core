using Microsoft.Extensions.DependencyInjection;
using System;

namespace SyZero.Util
{
    /// <summary>
    /// Autofac依赖注入服务
    /// </summary>
    public class SyZeroUtil
    {
        /// <summary>
        /// Autofac依赖注入静态服务
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 获取服务(Single)
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            T service;
            service = ServiceProvider.GetService<T>();
            return service;
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetRequiredService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 获取服务(请求生命周期内)
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetScopeService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
