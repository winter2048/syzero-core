using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using SyZero.Application.Service;
using SyZero.Client;
using SyZero.Feign.Proxy;
using SyZero.Serialization;
using SyZero.Service;

namespace SyZero.Feign
{
    /// <summary>
    /// Feign 服务注册器
    /// </summary>
    internal static class FeignServiceRegistrar
    {
        private static readonly FeignProxyFactoryManager _factoryManager = new FeignProxyFactoryManager();

        /// <summary>
        /// 注册 Feign 服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="feignOptions">配置选项</param>
        public static void Register(IServiceCollection services, FeignOptions feignOptions)
        {
            var definedTypes = ReflectionHelper.GetTypes();

            var baseFallback = typeof(IFallback);
            var baseType = typeof(IApplicationService);
            var types = definedTypes.Where(type => baseType.IsAssignableFrom(type) && type != baseType);
            var interfaceTypeInfos = types.Where(t => t.IsInterface);

            var implTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && !baseFallback.IsAssignableFrom(t) && t.IsVisible);

            var feignInterfaces = interfaceTypeInfos.Where(p => !p.IsGenericType && !implTypeInfos.Any(t => p.IsAssignableFrom(t)) && baseType.IsAssignableFrom(p));

            foreach (var targetType in feignInterfaces)
            {
                RegisterFeignProxy(services, targetType, feignOptions);
            }
        }

        /// <summary>
        /// 注册自定义代理工厂
        /// </summary>
        /// <param name="factory">代理工厂实例</param>
        public static void RegisterProxyFactory(IFeignProxyFactory factory)
        {
            _factoryManager.RegisterFactory(factory);
        }

        /// <summary>
        /// 注册单个 Feign 代理
        /// </summary>
        private static void RegisterFeignProxy(IServiceCollection services, Type targetType, FeignOptions feignOptions)
        {
            services.AddScoped(targetType, sp =>
            {
                var jsonSerialize = sp.GetRequiredService<IJsonSerialize>();
                var serviceManagement = sp.GetRequiredService<IServiceManagement>();

                var feignService = feignOptions.Service.FirstOrDefault(p => p.DllName == targetType.Assembly.GetName().Name);
                if (feignService == null)
                {
                    throw new Exception($"DLL:{targetType.Assembly.GetName().Name} 未在 Feign 配置中注册!");
                }

                // 合并全局配置
                MergeGlobalSettings(feignService, feignOptions.Global);

                var endPoint = GetServiceEndpoint(serviceManagement, feignService);
                
                // 使用工厂管理器创建代理
                return _factoryManager.CreateProxy(targetType, endPoint, feignService, jsonSerialize);
            });
        }

        /// <summary>
        /// 合并全局配置（服务配置优先）
        /// </summary>
        private static void MergeGlobalSettings(FeignService service, ServiceSetting global)
        {
            if (global == null) return;

            // 如果服务没有单独配置，则使用全局配置
            if (string.IsNullOrEmpty(service.Strategy) && !string.IsNullOrEmpty(global.Strategy))
            {
                service.Strategy = global.Strategy;
            }

            // 合并超时配置（默认值为 30，如果未修改则使用全局配置）
            if (service.Timeout == 30 && global.Timeout != 30)
            {
                service.Timeout = global.Timeout;
            }

            // 合并最大消息大小配置
            if (service.MaxMessageSize == 0 && global.MaxMessageSize > 0)
            {
                service.MaxMessageSize = global.MaxMessageSize;
            }

            // 合并重试次数配置
            if (service.Retry == 0 && global.Retry > 0)
            {
                service.Retry = global.Retry;
            }
        }

        /// <summary>
        /// 获取服务端点
        /// </summary>
        private static string GetServiceEndpoint(IServiceManagement serviceManagement, FeignService feignService)
        {
            var serviceList = serviceManagement.GetService(feignService.ServiceName)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
                
            if (serviceList == null || serviceList.Count == 0)
            {
                throw new Exception($"未找到服务: {feignService.ServiceName}");
            }

            var serviceInstance = serviceList.FirstOrDefault();

            // 根据协议类型构建端点
            var scheme = feignService.EnableSsl ? "https" : "http";
            return $"{scheme}://{serviceInstance.ServiceAddress}:{serviceInstance.ServicePort}";
        }
    }
}
