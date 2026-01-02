using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SyZero.Application.Routing;
using SyZero.Extension;

namespace SyZero.Feign
{
    public class RequestFeignHandler : DelegatingHandler
    {
        private string _serverName;

        public RequestFeignHandler(string serverName, HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
        {
            _serverName = serverName;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var builder = new UriBuilder(request.RequestUri);

            if (!builder.Path.StartsWith($"{RoutingHelper.ApiUrlPre}/"))
            {
                var controllerName = "";
                if (request.Options.TryGetValue(new HttpRequestOptionsKey<TypeInfo>(HttpRequestMessageOptions.InterfaceType), out var interfaceType))
                {
                    var interfaceName = RoutingHelper.GetControllerName(interfaceType.Name.Substring(1));
                    var customApiName = interfaceType.GetSingleAttributeOrDefaultByFullSearch<ApiAttribute>();
                    controllerName = customApiName?.Name ?? interfaceName;
                }

                builder.Path = $"/api/{_serverName}/{controllerName}/{builder.Path.RemovePreFix(RoutingHelper.ApiUrlPre)}".RemovePostFix("/");
            }
            else
            {
                builder.Path = builder.Path.RemovePreFix(RoutingHelper.ApiUrlPre);
            }

            request.RequestUri = builder.Uri;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
