using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Dynamitey;
using ImpromptuInterface;
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
            var asssAll = ReflectionHelper.GetAssemblies();
            var definedTypes = new List<TypeInfo>();
            foreach (var assembly in asssAll)
            {
                var definedType = assembly.DefinedTypes.ToList();
                foreach (var typeInfo in definedType)
                    definedTypes.Add(typeInfo);
            }

            var baseFallback = typeof(IFallback);
            var baseType = typeof(IApplicationService);
            var types = definedTypes.Where(typeInfo => baseType.IsAssignableFrom(typeInfo.AsType()) && typeInfo.AsType() != baseType);
            var interfaceTypeInfos = types.Where(t => t.IsInterface);

            var implTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && !baseFallback.IsAssignableFrom(t.AsType()));
            var fallbackTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && baseFallback.IsAssignableFrom(t.AsType()));
            var test = interfaceTypeInfos.Where(p => !p.AsType().IsGenericType && !implTypeInfos.Any(t => p.IsAssignableFrom(t)) && p.ImplementedInterfaces.Any(i => i == typeof(IApplicationService)));

            builder.RegisterType<ClientInterceptor>();

            foreach (var targetType in test)
            {
                var fallbackType = fallbackTypeInfos.FirstOrDefault(t => targetType.IsAssignableFrom(t.AsType()));
                if (fallbackType != null) {
                    builder
                       .RegisterType(fallbackType.AsType())
                       .As(targetType.AsType())
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

