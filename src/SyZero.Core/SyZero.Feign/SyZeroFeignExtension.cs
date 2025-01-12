using Castle.DynamicProxy;
using Dynamitey;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Application.Routing;
using SyZero.Application.Service;
using SyZero.Client;
using SyZero.Feign;
using SyZero.Runtime.Session;
using SyZero.Serialization;
using SyZero.Service;
using SyZero.Util;
using SyZero.Web.Common;


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

            var implTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && !baseFallback.IsAssignableFrom(t) && t.IsVisible);
            var fallbackTypeInfos = types.Where(t => t.IsClass && !t.IsAbstract && baseFallback.IsAssignableFrom(t));

            var test = interfaceTypeInfos.Where(p => !p.IsGenericType && !implTypeInfos.Any(t => p.IsAssignableFrom(t)) && baseType.IsAssignableFrom(p));

            foreach (var targetType in test)
            {
                var fallbackType = fallbackTypeInfos.FirstOrDefault(t => targetType.IsAssignableFrom(t));
                if (fallbackType != null)
                {
                    services.AddScoped(targetType, sp =>
                    {
                        FeignOptions feignOptions = AppConfig.GetSection<FeignOptions>("Feign");
                        IJsonSerialize jsonSerialize = SyZeroUtil.GetService<IJsonSerialize>();
                        ISySession sySession = SyZeroUtil.GetService<ISySession>();
                        IClient client = SyZeroUtil.GetService<IClient>();
                        IServiceManagement serviceManagement = SyZeroUtil.GetService<IServiceManagement>();

                        var feignService = feignOptions.Service.FirstOrDefault(p => p.DllName == targetType.Assembly.GetName().Name);
                        if (feignService == null)
                        {
                            throw new Exception($"DLL:{targetType.Assembly.GetName().Name}未注册!");
                        }

                        var services = serviceManagement.GetService(feignService.ServiceName).Result;

                        var endPoint = $"{services.FirstOrDefault().ServiceProtocol}://{services.FirstOrDefault().ServiceAddress}:{services.FirstOrDefault().ServicePort}";

                        var api = RestService.For(targetType, endPoint, new RefitSettings()
                        {
                            HttpMessageHandlerFactory = () =>
                            {
                                var responseHandler = new ResponseFeignHandler(feignService.ServiceName);
                                var authenticationHandler = new AuthenticationFeignHandler(feignService.ServiceName, responseHandler);
                                var requestHandler = new RequestFeignHandler(feignService.ServiceName, authenticationHandler);

                                return requestHandler;
                            },
                            DeserializationExceptionFactory = (httpResponse, exception) =>
                            {
                                Console.WriteLine(exception.Message);
                                return null;
                            },
                            ContentSerializer = new NewtonsoftJsonContentSerializer(
                                new JsonSerializerSettings
                                {
                                    Converters = { new LongToStrConverter() }
                                }
                            ),
                            ExceptionFactory = async (httpResponse) => {
                                if (httpResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                                {
                                    var jsonString = await httpResponse.Content.ReadAsStringAsync();
                                    var data = jsonSerialize.JSONToObject<SyMessageBoxModel>(jsonString);
                                    return new SyMessageException(data);
                                }
                                return null;
                            }
                    }
                        );
                        return api;
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
