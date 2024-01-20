using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyZero.Application.Service;
using SyZero.Client;
using SyZero.Feign;


namespace SyZero
{
    public static class SyZeroFeignExtension
    {

        /// <summary>
        /// 注册FeignModule
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSyZeroFeign(this IServiceCollection services)
        {
            var definedTypes = ReflectionHelper.GetTypes();

            var baseFallback = typeof(IFallback);
            var baseType = typeof(IApplicationService);
            var types = definedTypes.Where(type => baseType.IsAssignableFrom(type) && type != baseType);
            var interfaceTypeInfos = types.Where(t => t.IsInterface);

            var implTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && !baseFallback.IsAssignableFrom(t));
            var fallbackTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && baseFallback.IsAssignableFrom(t));

            var test = interfaceTypeInfos.Where(p => !p.IsGenericType && !implTypeInfos.Any(t => p.IsAssignableFrom(t)) && baseType.IsAssignableFrom(p));

            services.AddScoped<ClientInterceptor>();

            foreach (var targetType in test)
            {
                var fallbackType = fallbackTypeInfos.FirstOrDefault(t => targetType.IsAssignableFrom(t));
                if (fallbackType != null)
                {
                    services.AddScoped(targetType, fallbackType).AddScoped(sp =>
                    {
                        var proxyGenerator = new ProxyGenerator();
                        var target = sp.GetService(targetType);
                        var interceptor = sp.GetService<ClientInterceptor>();
                        return proxyGenerator.CreateInterfaceProxyWithTarget(target, interceptor);
                    });
                }
                else
                {
                    throw new Exception($"{targetType.Name}未实现Fallback！");
                }
            }
            return services;
        }
    }
}
