using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Server;
using SyZero.Application.Attributes;
using SyZero.Application.Service;
using SyZero.Client;
using SyZero.DynamicGrpc.Attributes;
using SyZero.DynamicGrpc.Helpers;

namespace SyZero.DynamicGrpc
{
    /// <summary>
    /// Dynamic gRPC 服务扩展方法
    /// </summary>
    public static class DynamicGrpcServiceExtensions
    {
        /// <summary>
        /// 缓存 MapGrpcService 方法
        /// </summary>
        private static MethodInfo _mapGrpcServiceMethod;

        /// <summary>
        /// 添加 Dynamic gRPC 服务到依赖注入容器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="options">配置选项</param>
        /// <returns>服务集合</returns>
        /// <exception cref="ArgumentNullException">options 为 null 时抛出</exception>
        public static IServiceCollection AddDynamicGrpc(this IServiceCollection services, DynamicGrpcOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Validate();

            // 注册选项
            services.AddSingleton(options);

            // 注册服务类型提供程序
            services.AddSingleton<DynamicGrpcServiceTypeProvider>();

            // 配置 Code-First gRPC (protobuf-net.Grpc)
            services.AddCodeFirstGrpc(config =>
            {
                if (options.MaxReceiveMessageSize.HasValue)
                {
                    config.MaxReceiveMessageSize = options.MaxReceiveMessageSize.Value;
                }
                if (options.MaxSendMessageSize.HasValue)
                {
                    config.MaxSendMessageSize = options.MaxSendMessageSize.Value;
                }
                config.EnableDetailedErrors = options.EnableDetailedErrors;
            });

            // 自动注册所有动态 gRPC 服务
            RegisterDynamicGrpcServices(services, options);

            return services;
        }

        /// <summary>
        /// 添加 Dynamic gRPC 服务到依赖注入容器（使用默认配置）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDynamicGrpc(this IServiceCollection services)
        {
            return AddDynamicGrpc(services, new DynamicGrpcOptions());
        }

        /// <summary>
        /// 添加 Dynamic gRPC 服务到依赖注入容器（使用配置委托）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="optionsAction">配置委托</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDynamicGrpc(this IServiceCollection services, Action<DynamicGrpcOptions> optionsAction)
        {
            var options = new DynamicGrpcOptions();
            optionsAction?.Invoke(options);
            return AddDynamicGrpc(services, options);
        }

        /// <summary>
        /// 映射所有动态 gRPC 服务端点
        /// </summary>
        /// <param name="endpoints">端点路由构建器</param>
        /// <returns>端点路由构建器</returns>
        public static IEndpointRouteBuilder MapDynamicGrpcServices(this IEndpointRouteBuilder endpoints)
        {
            var options = endpoints.ServiceProvider.GetRequiredService<DynamicGrpcOptions>();
            var serviceTypes = GetDynamicGrpcServiceTypes(options);

            foreach (var serviceType in serviceTypes)
            {
                MapGrpcServiceByType(endpoints, serviceType);
            }

            return endpoints;
        }

        /// <summary>
        /// 映射所有动态 gRPC 服务端点（带配置）
        /// </summary>
        /// <param name="endpoints">端点路由构建器</param>
        /// <param name="configureOptions">gRPC 服务端点配置</param>
        /// <returns>端点路由构建器</returns>
        public static IEndpointRouteBuilder MapDynamicGrpcServices(
            this IEndpointRouteBuilder endpoints,
            Action<object> configureOptions)
        {
            var options = endpoints.ServiceProvider.GetRequiredService<DynamicGrpcOptions>();
            var serviceTypes = GetDynamicGrpcServiceTypes(options);

            foreach (var serviceType in serviceTypes)
            {
                var builder = MapGrpcServiceByType(endpoints, serviceType);
                configureOptions?.Invoke(builder);
            }

            return endpoints;
        }

        /// <summary>
        /// 通过反射映射 gRPC 服务 (使用 protobuf-net.Grpc)
        /// </summary>
        private static object MapGrpcServiceByType(IEndpointRouteBuilder endpoints, Type serviceType)
        {
            // 获取或缓存 MapGrpcService 方法 (来自 ProtoBuf.Grpc.Server.ServicesExtensions)
            if (_mapGrpcServiceMethod == null)
            {
                _mapGrpcServiceMethod = typeof(ServicesExtensions)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m =>
                        m.Name == "MapGrpcService" &&
                        m.IsGenericMethod &&
                        m.GetParameters().Length == 1 &&
                        m.GetParameters()[0].ParameterType == typeof(IEndpointRouteBuilder));
            }

            if (_mapGrpcServiceMethod != null)
            {
                var genericMethod = _mapGrpcServiceMethod.MakeGenericMethod(serviceType);
                return genericMethod.Invoke(null, new object[] { endpoints });
            }

            return null;
        }

        /// <summary>
        /// 注册所有动态 gRPC 服务到 DI 容器
        /// </summary>
        private static void RegisterDynamicGrpcServices(IServiceCollection services, DynamicGrpcOptions options)
        {
            var serviceTypes = GetDynamicGrpcServiceTypes(options);

            foreach (var serviceType in serviceTypes)
            {
                // 获取服务实现的所有接口
                var interfaces = serviceType.GetInterfaces()
                    .Where(i => typeof(IDynamicApi).IsAssignableFrom(i) && i != typeof(IDynamicApi));

                foreach (var serviceInterface in interfaces)
                {
                    // 注册服务到 DI 容器
                    if (!services.Any(s => s.ServiceType == serviceInterface))
                    {
                        services.AddScoped(serviceInterface, serviceType);
                    }
                }

                // 同时注册具体类型
                if (!services.Any(s => s.ServiceType == serviceType))
                {
                    services.AddScoped(serviceType);
                }
            }
        }

        /// <summary>
        /// 获取所有动态 gRPC 服务类型
        /// </summary>
        private static IEnumerable<Type> GetDynamicGrpcServiceTypes(DynamicGrpcOptions options)
        {
            var assemblies = ReflectionHelper.GetAssemblies();
            var serviceTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(type => IsValidDynamicGrpcService(type.GetTypeInfo()));

                    serviceTypes.AddRange(types);
                }
                catch (ReflectionTypeLoadException)
                {
                    // 忽略无法加载的程序集
                }
            }

            return serviceTypes;
        }

        /// <summary>
        /// 验证类型是否为有效的动态 gRPC 服务
        /// </summary>
        private static bool IsValidDynamicGrpcService(TypeInfo typeInfo)
        {
            // 基本条件检查
            if (!typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType || !typeInfo.IsClass)
            {
                return false;
            }

            // 必须实现 IDynamicApi
            if (!typeof(IDynamicApi).IsAssignableFrom(typeInfo.AsType()))
            {
                return false;
            }

            // 不能是 IFallback 类型
            if (typeof(IFallback).IsAssignableFrom(typeInfo.AsType()))
            {
                return false;
            }

            // 必须有 DynamicApiAttribute 标记（DynamicApi 默认启用 gRPC）
            var dynamicApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicApiAttribute>(typeInfo);
            if (dynamicApiAttr == null)
            {
                return false;
            }

            // 不能有 NonDynamicApiAttribute 排除标记
            if (ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<NonDynamicApiAttribute>(typeInfo) != null)
            {
                return false;
            }

            // 不能有 NonGrpcServiceAttribute 排除标记（用于排除某个 DynamicApi 不注册为 gRPC）
            if (ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<NonGrpcServiceAttribute>(typeInfo) != null)
            {
                return false;
            }

            return true;
        }
    }
}
