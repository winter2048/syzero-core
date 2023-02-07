using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Dynamitey;
using ImpromptuInterface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using SyZero;
using SyZero.Application.Service;
using SyZero.Client;

namespace SyZero.Feign
{
    public class FeignModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var definedTypes = ReflectionHelper.GetTypes();

            var baseFallback = typeof(IFallback);
            var baseType = typeof(IApplicationService);
            var types = definedTypes.Where(type => baseType.IsAssignableFrom(type) && type != baseType);
            var interfaceTypeInfos = types.Where(t => t.IsInterface);

            var implTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && !baseFallback.IsAssignableFrom(t));
            var fallbackTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && baseFallback.IsAssignableFrom(t));

            var test = interfaceTypeInfos.Where(p => !p.IsGenericType && !implTypeInfos.Any(t => p.IsAssignableFrom(t)) && baseType.IsAssignableFrom(p));

            builder.RegisterType<ClientInterceptor>();

            foreach (var targetType in test)
            {
                var fallbackType = fallbackTypeInfos.FirstOrDefault(t => targetType.IsAssignableFrom(t));
                if (fallbackType != null) {
                    builder
                       .RegisterType(fallbackType)
                       .As(targetType)
                       .InterceptedBy(typeof(ClientInterceptor))
                       .EnableInterfaceInterceptors();
                }
                else
                {
                    throw new Exception($"{targetType.Name}未实现Fallback！");
                }
            }
           
        }
    }
}

