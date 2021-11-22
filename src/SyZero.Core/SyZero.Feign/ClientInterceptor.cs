using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyZero.Application.Routing;
using SyZero.Client;
using SyZero.Serialization;
using SyZero.Util;

namespace SyZero.Feign
{
    public class ClientInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"Feign start:{invocation.Method.Name}");
            try
            {
                if (true)
                {
                    FeignOptions feignOptions = AppConfig.GetSection<FeignOptions>("Feign");
                    IJsonSerialize jsonSerialize = AutofacUtil.GetService<IJsonSerialize>();
                    Console.WriteLine($"Feign arguments:{jsonSerialize.ObjectToJSON(invocation.Arguments)}");
                    var url = RoutingHelper.GetRouteUrlByInterface(feignOptions.Service.FirstOrDefault(p => p.DllName == invocation.TargetType.Assembly.GetName().Name)?.ServiceName, invocation.Method);
                    Console.WriteLine($"Feign url:{url}");
                    var apiMethodAttribute = ReflectionHelper.GetSingleAttributeOrDefault<ApiMethodAttribute>(invocation.Method);
                    var paramInfo = invocation.Method.GetParameters();
                    var parameterType = paramInfo.Select(it => it.ParameterType).ToArray();

                 

                    var returnType = invocation.Method.ReturnType;
                    IClient client = AutofacUtil.GetService<IClient>();


                    RequestTemplate requestTemplate = new RequestTemplate(apiMethodAttribute.HttpMethod, url);
                    ResponseTemplate responseTemplate = client.ExecuteAsync(requestTemplate, new System.Threading.CancellationToken()).Result;


                    //throw new Exception();
                    invocation.ReturnValue = invocation.Arguments[0];
                }
                Console.WriteLine($"Feign end:{invocation.Method.Name} | ReturnValue: Success = {invocation.ReturnValue}");
            }
            catch
            {
                invocation.Proceed();
                Console.WriteLine($"Feign end:{invocation.Method.Name} | ReturnValue: Fallback = {invocation.ReturnValue}");
            }

        }
    }
}
