using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyZero.Application.Routing;
using SyZero.Client;
using SyZero.Runtime.Session;
using SyZero.Serialization;
using SyZero.Service;
using SyZero.Util;

namespace SyZero.Feign
{
    public class ClientInterceptor : IInterceptor
    {
        private IInvocation _invocation;
        public void Intercept(IInvocation invocation)
        {
            this._invocation = invocation;
            Console.WriteLine($"Feign start:{invocation.Method.Name}");
            try
            {
                FeignOptions feignOptions = AppConfig.GetSection<FeignOptions>("Feign");
                IJsonSerialize jsonSerialize = AutofacUtil.GetService<IJsonSerialize>();
                ISySession sySession = AutofacUtil.GetService<ISySession>();
                IClient client = AutofacUtil.GetService<IClient>();
                IServiceManagement serviceManagement = AutofacUtil.GetService<IServiceManagement>();
                Console.WriteLine($"Feign arguments:{jsonSerialize.ObjectToJSON(invocation.Arguments)}");

                var feignService = feignOptions.Service.FirstOrDefault(p => p.DllName == invocation.TargetType.Assembly.GetName().Name);
                if (feignService == null)
                {
                    throw new Exception($"DLL:{invocation.TargetType.Assembly.GetName().Name}未注册!");
                }

                var services = serviceManagement.GetService(feignService.ServiceName).Result;

                var endPoint = $"{services.FirstOrDefault().ServiceProtocol}://{services.FirstOrDefault().ServiceAddress}:{services.FirstOrDefault().ServicePort}";
                var url = RoutingHelper.GetRouteUrlByInterface(endPoint, feignService.ServiceName, invocation.Method);
                Console.WriteLine($"Feign url:{url}");
                var apiMethodAttribute = ReflectionHelper.GetSingleAttributeOrDefault<ApiMethodAttribute>(invocation.Method);
                var paramInfo = invocation.Method.GetParameters();
                var parameterType = paramInfo.Select(it => it.ParameterType).ToArray();

                var returnType = GetReturnType();
                
                RequestTemplate requestTemplate = new RequestTemplate(apiMethodAttribute.HttpMethod, url);
                requestTemplate.Headers.Add("Authorization", sySession.Token ?? "");
                if (invocation.Arguments.Length > 0)
                {
                    requestTemplate.Body = jsonSerialize.ObjectToJSON(invocation.Arguments[0]);
                }

                var cxecuteAsync = client.GetType().GetMethod("ExecuteAsync").MakeGenericMethod(new Type[] { returnType });
                Task responseTemplateTask = cxecuteAsync.Invoke(client, new object[] { requestTemplate, new System.Threading.CancellationToken() }) as Task;
                responseTemplateTask.Wait();
                var responseTemplate = (dynamic)responseTemplateTask.GetType().GetProperty("Result").GetValue(responseTemplateTask, null);
                if (responseTemplate.HttpStatusCode != System.Net.HttpStatusCode.OK || responseTemplate.Code != SyMessageBoxStatus.Success)
                {
                    throw new SyMessageException(responseTemplate.Msg, responseTemplate.Code);
                }
                else
                {
                    invocation.ReturnValue = GetReturnValue(responseTemplate.Body);
                }
                Console.WriteLine($"Feign end:{invocation.Method.Name} | ReturnValue: Success = {invocation.ReturnValue}");
            }
            catch (Exception ex)
            {
                invocation.Proceed();
                if (ex is SyMessageException)
                {
                    throw ex;
                }
                else
                {
                    Console.WriteLine($"Feign end:{invocation.Method.Name} | ReturnValue: Fallback = {invocation.ReturnValue}");
                }
            }
        }

        public Task<T> GetTask<T>(T t)
        {
            return Task.Run(() =>
            {
                return t;
            });
        }

        private bool IsReturnTypeTask()
        {
            return _invocation.Method.ReturnType.BaseType == typeof(Task);
        }

        private Type GetReturnType()
        {
            var returnType = _invocation.Method.ReturnType;
            if (IsReturnTypeTask())
            {
                returnType = returnType.GenericTypeArguments[0];
            }
            return returnType;
        }

        private object GetReturnValue(object returnValue)
        {
            if (IsReturnTypeTask())
            {
                var mi = this.GetType().GetMethod("GetTask").MakeGenericMethod(new Type[] { GetReturnType() });
                return mi.Invoke(this, new object[] { returnValue });
            }
            return returnValue;
        }
    }
}
