using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SyZero.Application.Routing;
using SyZero.Runtime.Session;
using SyZero.Util;

namespace SyZero.Feign
{
    public class AuthenticationFeignHandler : DelegatingHandler
    {
        private string _serverName;

        public AuthenticationFeignHandler(string serverName, HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
        {
            _serverName = serverName;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ISySession sySession = SyZeroUtil.GetService<ISySession>();

            // 在请求中添加身份验证头
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sySession.Token ?? "");

            // 调用下一个处理程序
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
